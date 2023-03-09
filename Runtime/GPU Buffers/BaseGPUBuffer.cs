using UnityEngine;

namespace SH.MapGenerator.GPUBuffers
{
    public abstract class BaseGPUBuffer : ScriptableObject
    {
        public ComputeBuffer Buffer { get; protected set; }
        public int Size { get; protected set; }

        public abstract void Init(int size);
        public abstract void DeInit();

        public int GetAmountOfAllocatedBytes()
        {
            return Buffer.count * Buffer.stride;
        }
    }
}