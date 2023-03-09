using UnityEngine;
using SH.MapGenerator.GPUBuffers;

namespace SH.MapGenerator.Generators
{
    [System.Serializable]
    public abstract class BaseGenerator
    {
        [field: SerializeField] public bool Enabled { get; private set; } = true;

        public abstract void Generate(RuntimeData data);
        public abstract BaseGPUBuffer[] GetAllGPUBuffers();
        public virtual TerrainLayer[] GetAllTerrainLayers() => new TerrainLayer[0];

        protected void DispatchComputeShader(ComputeShader compute, int kernel, int width, int height)
        {
            compute.GetKernelThreadGroupSizes(kernel, out uint sizeX, out uint sizeY, out _);
            compute.Dispatch(kernel, Mathf.CeilToInt((float)width / sizeX), Mathf.CeilToInt((float)height / sizeY), 1);
        }
    }
}