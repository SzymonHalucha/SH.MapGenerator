using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class PowerModifierGenerator : BaseModifierGenerator
    {
        [SerializeField, Range(0, 16f)] private float power = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Power");
            int kernel = shader.FindKernel("Power");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Power", power);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}