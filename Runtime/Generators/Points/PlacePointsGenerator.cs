using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Points
{
    public class PlacePointsGenerator : BaseGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("PlacePoints");
            int kernel = shader.FindKernel("PlacePoints");
            int pointsSqrt = Mathf.CeilToInt(Mathf.Sqrt(pointsBuffer.Size));

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_PointsCountSqrt", pointsSqrt);
            shader.SetBuffer(kernel, "PointsBuffer", pointsBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, pointsSqrt, pointsSqrt);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer, pointsBuffer };
        }
    }
}