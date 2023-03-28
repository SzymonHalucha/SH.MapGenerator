using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Blends
{
    public abstract class BaseBlendGenerator : BaseGenerator
    {
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer blendBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(0, 1f)] private float weight = 1f;

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer, blendBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { boundsBuffer };
        }

        protected virtual void SetupComputeShader(string kernelName)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Blends");
            int kernel = shader.FindKernel(kernelName);

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetFloat("_Weight", weight);
            shader.SetVector("_StartOffset", boundsBuffer != null ? boundsBuffer.StartOffset : Vector2.zero);
            shader.SetBuffer(kernel, "BlendBuffer", blendBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            if (boundsBuffer != null)
                DispatchComputeShader(shader, kernel, boundsBuffer.Size, boundsBuffer.Size);
            else
                DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }
    }
}