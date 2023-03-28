using UnityEngine;
using UnityEditor;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Editor
{
    [CustomEditor(typeof(BaseCPUBuffer), true)]
    public class BaseCPUBufferEditor : UnityEditor.Editor
    {
        private const int scalar = 1024;
        private BaseCPUBuffer buffer;

        private void OnEnable()
        {
            buffer = (BaseCPUBuffer)target;
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
                    buffer.Dispose();
            }
            else
            {
                EditorGUILayout.LabelField("Data is not initialized.");
            }
        }
    }
}