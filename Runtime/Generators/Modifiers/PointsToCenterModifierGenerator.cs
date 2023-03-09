using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class PointsToCenterModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField, Range(0, 1f)] private float threshold = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("PointsToCenter");
            int kernel = shader.FindKernel("PointsToCenter");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetInt("_PointsCount", pointsBuffer.Size);
            shader.SetFloat("_Threshold", threshold);
            shader.SetBuffer(kernel, "PointsBuffer", pointsBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer, TargetBuffer };
        }
    }
}