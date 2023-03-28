using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class NormalizeGenerator : BaseGenerator
    {
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;

        public override void Generate(RuntimeData data)
        {
            if (boundsBuffer != null && boundsBuffer.Size == 0)
                return;

            ComputeBuffer minMaxBuffer = GetMinMaxBuffer(targetBuffer);
            ComputeShader shader = ComputeShadersContrainer.GetShader("Normalize");
            int kernel = shader.FindKernel("Normalize");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetVector("_StartOffset", boundsBuffer != null ? boundsBuffer.StartOffset : Vector2.zero);
            shader.SetBuffer(kernel, "MinMaxBuffer", minMaxBuffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            if (boundsBuffer != null)
                DispatchComputeShader(shader, kernel, boundsBuffer.Size, boundsBuffer.Size);
            else
                DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);

            minMaxBuffer.Release();
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { boundsBuffer };
        }

        private ComputeBuffer GetMinMaxBuffer(Float2DArrayGPUBuffer bufferData)
        {
            ComputeBuffer minMaxBuffer = new ComputeBuffer(2, sizeof(int));
            minMaxBuffer.SetData(new int[] { int.MaxValue, int.MinValue });

            ComputeShader shader = ComputeShadersContrainer.GetShader("Normalize");
            int kernel = shader.FindKernel("MinMax");

            shader.SetInt("_Size", bufferData.Width);
            shader.SetVector("_StartOffset", boundsBuffer != null ? boundsBuffer.StartOffset : Vector2.zero);
            shader.SetBuffer(kernel, "TargetBuffer", bufferData.Buffer);
            shader.SetBuffer(kernel, "MinMaxBuffer", minMaxBuffer);

            if (boundsBuffer != null)
                DispatchComputeShader(shader, kernel, boundsBuffer.Size, boundsBuffer.Size);
            else
                DispatchComputeShader(shader, kernel, bufferData.Width, bufferData.Height);

            return minMaxBuffer;
        }
    }
}