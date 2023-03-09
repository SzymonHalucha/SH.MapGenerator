using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class DistortionModifierGenerator : BaseModifierGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer originBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer distortionBuffer = null;
        [SerializeField, Range(0, 1f)] private float strength = 0.1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Distortion");
            int kernel = shader.FindKernel("Distortion");

            shader.SetInt("_Size", TargetBuffer.Width);
            shader.SetFloat("_Strength", strength);
            shader.SetBuffer(kernel, "OriginBuffer", originBuffer.Buffer);
            shader.SetBuffer(kernel, "DistortionBuffer", distortionBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", TargetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, TargetBuffer.Width, TargetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { originBuffer, distortionBuffer, TargetBuffer };
        }
    }
}