using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Noises
{
    public class PerlinGenerator : BaseNoiseGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField, Range(1, 16)] private int octaves = 4;
        [SerializeField, Range(0, 512f)] private float scale = 30f;
        [SerializeField, Range(0, 1f)] private float persistence = 0.5f;
        [SerializeField, Range(0, 16f)] private float lacunarity = 2f;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Perlin");
            int kernel = shader.FindKernel("Perlin");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_Seed", data.Random.Range(int.MaxValue));
            shader.SetInt("_Octaves", octaves);
            shader.SetFloat("_Scale", scale);
            shader.SetFloat("_Persistence", persistence);
            shader.SetFloat("_Lacunarity", lacunarity);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer };
        }
    }
}