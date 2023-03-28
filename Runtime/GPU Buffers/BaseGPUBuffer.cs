using UnityEngine;

namespace SH.MapGenerator.GPUBuffers
{
    public abstract class BaseGPUBuffer : ScriptableObject, System.IDisposable
    {
        public ComputeBuffer Buffer { get; protected set; }
        public int Size { get; protected set; }

        public abstract void Init(int size);
        public abstract void Dispose();

        public int GetAmountOfAllocatedBytes()
        {
            return Buffer.count * Buffer.stride;
        }
    }
}