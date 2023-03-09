using Stopwatch = System.Diagnostics.Stopwatch;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace SH.MapGenerator.Editor
{
    [CustomEditor(typeof(MapGeneratorBehaviour))]
    public class MapGeneratorBehaviourEditor : UnityEditor.Editor
    {
        private readonly MethodInfo initMethod = typeof(MapGeneratorBehaviour).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly MethodInfo generateWholeMapMethod = typeof(MapGeneratorBehaviour).GetMethod("GenerateWholeMap", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly MethodInfo deInitMethod = typeof(MapGeneratorBehaviour).GetMethod("DeInit", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly FieldInfo terrainField = typeof(MapGeneratorBehaviour).GetField("terrain", BindingFlags.NonPublic | BindingFlags.Instance);
        private Stopwatch stopwatch = null;
        private MapGeneratorBehaviour generator = null;
        private MapGeneratorData data = null;
        private Texture2D texture = null;

        private void OnEnable()
        {
            data = (MapGeneratorData)serializedObject.FindProperty("data").objectReferenceValue;
            generator = (MapGeneratorBehaviour)target;
            stopwatch = new Stopwatch();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (data == null)
                return;

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Texture"))
                GenerateHeightMap();

            if (GUILayout.Button("Generate Terrain"))
                GenerateTerrain();

            if (texture == null)
                return;

            EditorGUILayout.Space();
            EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(Screen.width, Screen.width), texture, null, ScaleMode.ScaleToFit, 1.0f);
        }

        private void SetPreviewTexture(float[,] values)
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

        private void GenerateHeightMap()
        {
            stopwatch.Restart();
            stopwatch.Start();
            initMethod.Invoke(generator, new object[] { (int)System.DateTime.Now.Ticks });
            var map = ((float[,] HeightMap, float[,,] SplatMap))generateWholeMapMethod.Invoke(generator, null);
            deInitMethod.Invoke(generator, null);
            stopwatch.Stop();

            if (map.HeightMap != null)
                SetPreviewTexture(map.HeightMap);

            Debug.Log($"Generated island in {stopwatch.Elapsed.TotalMilliseconds} ms.");
        }

        private void GenerateTerrain()
        {
            stopwatch.Restart();
            stopwatch.Start();
            generator.GenerateTerrain((int)System.DateTime.Now.Ticks);
            stopwatch.Stop();

            Terrain terrain = (Terrain)terrainField.GetValue(generator);
            SetPreviewTexture(terrain.terrainData.GetHeights(0, 0, data.HeightMapSize, data.HeightMapSize));
            Debug.Log($"Generated terrain in {stopwatch.Elapsed.TotalMilliseconds} ms.");
        }
    }
}