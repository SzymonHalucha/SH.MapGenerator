using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class NormalizeModifierGenerator : BaseModifierGenerator
    {
        public override void Generate(RuntimeData data)
        {
            ComputeBuffer minMaxBuffer = GetMinMaxBuffer(TargetBuffer);
            ComputeShader shader = ComputeShadersContrainer.GetShader("Normalize");
            int kernel = shader.FindKernel("Normalize");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetBuffer(kernel, "MinMaxBuffer", minMaxBuffer);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
            minMaxBuffer.Release();
        }

        private ComputeBuffer GetMinMaxBuffer(Float2DArrayGPUBuffer bufferData)
        {
            ComputeBuffer minMaxBuffer = new ComputeBuffer(2, sizeof(int));
            minMaxBuffer.SetData(new int[] { int.MaxValue, int.MinValue });

            ComputeShader shader = ComputeShadersContrainer.GetShader("Normalize");
            int kernel = shader.FindKernel("MinMax");

            shader.SetInt("_Size", bufferData.Width);
            shader.SetBuffer(kernel, "TargetBuffer", bufferData.Buffer);
            shader.SetBuffer(kernel, "MinMaxBuffer", minMaxBuffer);

            DispatchComputeShader(shader, kernel, bufferData.Width, bufferData.Height);
            return minMaxBuffer;
        }
    }
}