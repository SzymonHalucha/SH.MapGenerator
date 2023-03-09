using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class ClampModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private float min = 0;
        [SerializeField] private float max = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Clamp");
            int kernel = shader.FindKernel("Clamp");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Min", min);
            shader.SetFloat("_Max", max);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}