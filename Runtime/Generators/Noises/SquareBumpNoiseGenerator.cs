using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using SH.MapGenerator.Utils;
using Bounds = SH.MapGenerator.Utils.Bounds;

namespace SH.MapGenerator.Generators.Noises
{
    public class SquareBumpNoiseGenerator : BaseGenerator
    {
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(0, 16f)] private float radius = 1f;

        public override void Generate(RuntimeData data)
        {
            if (boundsBuffer != null && boundsBuffer.Size == 0)
                return;

            ComputeShader shader = ComputeShadersContrainer.GetShader("SquareBumpNoise");
            int kernel = shader.FindKernel("SquareBumpNoise");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetFloat("_Radius", radius);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            if (boundsBuffer != null)
            {
                foreach (Bounds bounds in boundsBuffer.Bounds)
                {
                    shader.SetInt("_PartialSize", bounds.Size);
                    shader.SetVector("_StartOffset", bounds.StartOffset);
                    DispatchComputeShader(shader, kernel, bounds.Size, bounds.Size);
                }
            }
            else
            {
                shader.SetInt("_PartialSize", targetBuffer.Width);
                shader.SetVector("_StartOffset", Vector2.zero);
                DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
            }
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