using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class GaussianBlurGenerator : BaseGenerator
    {
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer originBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(0, 32f)] private float sigma = 2f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("GaussianBlur");
            int kernel = shader.FindKernel("GaussianBlur");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetFloat("_Sigma", sigma);
            shader.SetInt("_KernelRadius", Mathf.CeilToInt(3f * sigma));
            shader.SetVector("_StartOffset", boundsBuffer != null ? boundsBuffer.StartOffset : Vector2.zero);
            shader.SetBuffer(kernel, "OriginBuffer", originBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            if (boundsBuffer != null)
                DispatchComputeShader(shader, kernel, boundsBuffer.Size, boundsBuffer.Size);
            else
                DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { originBuffer, targetBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { boundsBuffer };
        }
    }
}