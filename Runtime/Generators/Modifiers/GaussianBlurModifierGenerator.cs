using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class GaussianBlurModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer originBuffer = null;
        [SerializeField, Range(0, 32f)] private float sigma = 2f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("GaussianBlur");
            int kernel = shader.FindKernel("GaussianBlur");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Sigma", sigma);
            shader.SetInt("_KernelRadius", Mathf.CeilToInt(3f * sigma));
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