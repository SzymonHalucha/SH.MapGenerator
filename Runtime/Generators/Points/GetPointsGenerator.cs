using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Generators.Points
{
    public class GetPointsGenerator : BaseGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsGPUBuffer = null;
        [SerializeField] private Vector1DArrayCPUBuffer pointsCPUBuffer = null;

        public override void Generate(RuntimeData data)
        {
            pointsCPUBuffer.Init(pointsGPUBuffer.GetData());
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsGPUBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { pointsCPUBuffer };
        }
    }
}