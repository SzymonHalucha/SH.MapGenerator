using Stopwatch = System.Diagnostics.Stopwatch;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using SH.MapGenerator.Generators;
using SH.MapGenerator.GPUBuffers;

namespace SH.MapGenerator.Editor
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : UnityEditor.Editor
    {
        private readonly FieldInfo noisesField = typeof(MapGenerator).GetField("noises", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly MethodInfo initializePipelineMethod = typeof(MapGenerator).GetMethod("InitializePipeline", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly MethodInfo deinitializePipelineMethod = typeof(MapGenerator).GetMethod("DeinitializePipeline", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly MethodInfo initializeAllGPUBuffersMethod = typeof(MapGenerator).GetMethod("InitializeAllGPUBuffers", BindingFlags.NonPublic | BindingFlags.Instance);

        private bool useRandomSeed;
        private int seed;
        private Object heightMapBuffer;
        private Stopwatch stopwatch;
        private Texture2D texture;

        private void OnEnable()
        {
            stopwatch = new Stopwatch();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor Tools", EditorStyles.boldLabel);

            useRandomSeed = EditorGUILayout.Toggle("Use Random Seed", useRandomSeed);
            seed = EditorGUILayout.IntField("Seed", seed);
            heightMapBuffer = EditorGUILayout.ObjectField("Height Map Buffer", heightMapBuffer, typeof(Float2DArrayGPUBuffer), false);

            if (GUILayout.Button("Run Pipeline"))
                RunPipelineEditor();

            if (texture == null)
                return;

            EditorGUILayout.Space();
            EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(Screen.width, Screen.width), texture, null, ScaleMode.ScaleToFit, 1.0f);
        }

        private void RunPipelineEditor()
        {
            stopwatch.Reset();
            stopwatch.Start();

            int currentSeed = useRandomSeed ? (int)System.DateTime.Now.Ticks : seed;
            RuntimeData runtimeData = (RuntimeData)initializePipelineMethod.Invoke(((MapGenerator)target), new object[] { currentSeed });
            initializeAllGPUBuffersMethod.Invoke(((MapGenerator)target), new object[] { runtimeData });

            NoiseData[] noises = (NoiseData[])noisesField.GetValue(((MapGenerator)target));

            foreach (NoiseData noise in noises)
                if (noise.Enabled)
                    foreach (BaseGenerator generator in noise.Generators)
                        if (generator.Enabled)
                            generator.Generate(runtimeData);

            float[,] values = null;
            if (heightMapBuffer != null)
                values = ((Float2DArrayGPUBuffer)heightMapBuffer).GetData();

            deinitializePipelineMethod.Invoke(((MapGenerator)target), new object[] { runtimeData });

            stopwatch.Stop();
            Debug.Log("Generation Time: " + stopwatch.Elapsed.TotalMilliseconds + "ms.");

            if (values != null)
                SetTexturePreview(values);
        }

        private void SetTexturePreview(float[,] values)
        {
            int sizeX = values.GetLength(0);
            int sizeY = values.GetLength(1);

            if (texture == null || texture.width != sizeX || texture.height != sizeY)
            {
                texture = new Texture2D(sizeX, sizeY, TextureFormat.RGBA32, false, true);
                texture.filterMode = FilterMode.Point;
            }

            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                    texture.SetPixel(x, y, new Color(values[x, y], values[x, y], values[x, y], 1f));

            texture.Apply();
        }
    }
}