using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class ClearModifierGenerator : BaseModifierGenerator
    {
        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Clear");
            int kernel = shader.FindKernel("Clear");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}