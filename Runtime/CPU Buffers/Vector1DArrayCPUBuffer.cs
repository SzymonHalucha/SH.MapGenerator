using UnityEngine;

namespace SH.MapGenerator.CPUBuffers
{
    [CreateAssetMenu(menuName = "SH/Map Generator/CPU Buffers/Vector 1D Array", fileName = "New Vector 1D Array Buffer", order = 1)]
    public class Vector1DArrayCPUBuffer : BaseCPUBuffer
    {
        public Vector3[] Vectors { get; private set; }

        public void Init(int size)
        {
            Size = size;
            Vectors = new Vector3[Size];
        }

        public void Init(Vector3[] array)
        {
            Size = array.Length;
            Vectors = array;
        }

        public override void Dispose()
        {
            Size = 0;
            Vectors = null;
        }

        public override int GetAmountOfAllocatedBytes()
        {
            return Vectors.Length * sizeof(float) * 3;
        }
    }
}