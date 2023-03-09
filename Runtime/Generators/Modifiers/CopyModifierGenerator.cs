using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class CopyModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer originBuffer = null;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Copy");
            int kernel = shader.FindKernel("Copy");

            shader.SetInt("_Size", TargetBuffer.Width);
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