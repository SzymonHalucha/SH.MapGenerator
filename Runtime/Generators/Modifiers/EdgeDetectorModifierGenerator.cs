using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class EdgeDetectorModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer originBuffer = null;
        [SerializeField, Range(0, 1f)] private float threshold = 0.5f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("EdgeDetector");
            int kernel = shader.FindKernel("EdgeDetector");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Threshold", threshold);
            shader.SetBuffer(kernel, "OriginBuffer", originBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { originBuffer, TargetBuffer };
        }
    }
}