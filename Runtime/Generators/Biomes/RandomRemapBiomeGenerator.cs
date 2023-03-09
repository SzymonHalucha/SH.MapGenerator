using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Biomes
{
    public class RandomRemapBiomeGenerator : BaseBiomeGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer originBuffer = null;
        [SerializeField, Range(0, 64)] private int minRange = 0;
        [SerializeField, Range(0, 64)] private int maxRange = 0;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("RandomRemap");
            int kernel = shader.FindKernel("RandomRemap");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_Seed", data.Random.Range(int.MaxValue));
            shader.SetInt("_MinRange", minRange);
            shader.SetInt("_MaxRange", maxRange);
            shader.SetBuffer(kernel, "MaskBuffer", originBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer, originBuffer };
        }
    }
}