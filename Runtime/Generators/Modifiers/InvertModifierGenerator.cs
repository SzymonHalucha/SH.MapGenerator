using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class InvertModifierGenerator : BaseModifierGenerator
    {
        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Invert");
            int kernel = shader.FindKernel("Invert");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}