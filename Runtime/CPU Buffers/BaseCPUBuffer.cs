using UnityEngine;

namespace SH.MapGenerator.CPUBuffers
{
    public abstract class BaseCPUBuffer : ScriptableObject, System.IDisposable
    {
        public int Size { get; protected set; }

        public abstract void Dispose();
        public abstract int GetAmountOfAllocatedBytes();
    }
}