using UnityEngine;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class CutoffModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private float minCutoff = 0;
        [SerializeField] private float maxCutoff = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Cutoff");
            int kernel = shader.FindKernel("Cutoff");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Min", minCutoff);
            shader.SetFloat("_Max", maxCutoff);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }
    }
}