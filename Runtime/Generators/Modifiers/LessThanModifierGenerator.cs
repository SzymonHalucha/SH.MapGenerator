using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class LessThanModifierGenerator : BaseModifierGenerator
    {
        [SerializeField, Range(0, 1f)] private float threshold = 0.5f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("LessThan");
            int kernel = shader.FindKernel("LessThan");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Threshold", threshold);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}