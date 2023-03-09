using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Blends
{
    public abstract class BaseBlendGenerator : BaseGenerator
    {
        [SerializeField] protected Float2DArrayGPUBuffer TargetBuffer = null;
        [SerializeField] protected Float2DArrayGPUBuffer BlendBuffer = null;
        [SerializeField, Range(0, 1f)] protected float Weight = 1f;

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { TargetBuffer, BlendBuffer };
        }

        protected virtual void SetupComputeShader(string kernelName)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Blends");
            int kernel = shader.FindKernel(kernelName);

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Weight", Weight);
            shader.SetVector("_StartOffset", Vector2.zero);
            shader.SetBuffer(kernel, "BlendBuffer", BlendBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}