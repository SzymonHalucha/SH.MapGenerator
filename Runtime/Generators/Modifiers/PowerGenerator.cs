using UnityEngine;
using SH.MapGenerator.Utils;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Generators.Modifiers
{
    public class PowerGenerator : BaseGenerator
    {
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(0, 16f)] private float power = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Power");
            int kernel = shader.FindKernel("Power");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetFloat("_Power", power);
            shader.SetVector("_StartOffset", boundsBuffer != null ? boundsBuffer.StartOffset : Vector2.zero);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            if (boundsBuffer != null)
                DispatchComputeShader(shader, kernel, boundsBuffer.Size, boundsBuffer.Size);
            else
                DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { boundsBuffer };
        }
    }
}