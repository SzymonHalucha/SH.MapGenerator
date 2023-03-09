// using UnityEngine;

// namespace SH.MapGenerator.CPUBuffers
// {
//     [CreateAssetMenu(menuName = "SH/Map Generator/CPU Buffers/Bounds", fileName = "New Bounds Buffer", order = 1)]
//     public class BoundsCPUBuffer : ScriptableObject
//     {
//         [field: SerializeField, Sirenix.OdinInspector.ReadOnly] public int Index { get; private set; } //Delete Attributes
//         [field: SerializeField, Sirenix.OdinInspector.ReadOnly] public Vector2Int Center { get; private set; } //Delete Attributes
//         [field: SerializeField, Sirenix.OdinInspector.ReadOnly] public int Size { get; private set; } //Delete Attributes

//         public Vector2 StartOffset => Center - new Vector2(Size / 2, Size / 2);

//         public void Init(int index, Vector2Int center, int size)
//         {
//             Index = index;
//             Center = center;
//             Size = size;
//         }
//     }
// }