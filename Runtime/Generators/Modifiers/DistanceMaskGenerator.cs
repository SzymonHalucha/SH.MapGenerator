using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class DistanceMaskGenerator : BaseGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(0, 1f)] private float threshold = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("DistanceMask");
            int kernel = shader.FindKernel("DistanceMask");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_PointsCount", pointsBuffer.Size);
            shader.SetFloat("_Threshold", threshold);
            shader.SetBuffer(kernel, "PointsBuffer", pointsBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer, targetBuffer };
        }
    }
}