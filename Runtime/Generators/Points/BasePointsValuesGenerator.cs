using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Points
{
    public abstract class BasePointsValuesGenerator : BaseGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer maskBuffer = null;
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;

        protected void SetupComputeShader(string kernelName)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Points");
            int kernel = shader.FindKernel(kernelName);
            int pointsSqrt = Mathf.CeilToInt(Mathf.Sqrt(pointsBuffer.Size));

            shader.SetInt("_Size", maskBuffer.Width);
            shader.SetInt("_PointsCountSqrt", pointsSqrt);
            shader.SetBuffer(kernel, "MaskBuffer", maskBuffer.Buffer);
            shader.SetBuffer(kernel, "PointsBuffer", pointsBuffer.Buffer);

            DispatchComputeShader(shader, kernel, pointsSqrt, pointsSqrt);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { maskBuffer, pointsBuffer };
        }
    }
}