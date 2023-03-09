using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class StepModifierGenerator : BaseModifierGenerator
    {
        [SerializeField, Range(0, 1f)] private float step = 0.5f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Step");
            int kernel = shader.FindKernel("Step");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Step", step);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}