using UnityEngine;
using UnityEditor;
using SH.MapGenerator.GPUBuffers;

namespace SH.MapGenerator.Editor
{
    [CustomEditor(typeof(BaseGPUBuffer), true)]
    public class BaseGPUBufferEditor : UnityEditor.Editor
    {
        private const int scalar = 1024;
        private BaseGPUBuffer buffer;

        private void OnEnable()
        {
            buffer = (BaseGPUBuffer)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            if (buffer.Size != 0)
            {
                float size = (float)buffer.GetAmountOfAllocatedBytes() / scalar;

                if (size >= scalar)
                    EditorGUILayout.LabelField($"Data is initialized with {buffer.Size} elements. (Allocated: {size / scalar} MB)");
                else
                    EditorGUILayout.LabelField($"Data is initialized with {buffer.Size} elements. (Allocated: {size} KB)");

                if (GUILayout.Button("Deinitialize"))
                    buffer.DeInit();
            }
            else
            {
                EditorGUILayout.LabelField("Data is not initialized.");
            }
        }
    }
}