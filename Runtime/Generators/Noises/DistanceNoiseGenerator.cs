using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Noises
{
    public class DistanceNoiseGenerator : BaseNoiseGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(0, 16f)] private float radius = 1f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("DistanceNoise");
            int kernel = shader.FindKernel("DistanceNoise");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetFloat("_Radius", radius);
            shader.SetVector("_Center", Vector2.zero);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer };
        }
    }
}