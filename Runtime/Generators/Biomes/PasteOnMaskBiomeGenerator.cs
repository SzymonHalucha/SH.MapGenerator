using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Biomes
{
    public class PasteOnMaskBiomeGenerator : BaseBiomeGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer biomeBuffer = null;
        [SerializeField] private int maskIndex = 0;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("PasteOnMask");
            int kernel = shader.FindKernel("PasteOnMask");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_Index", maskIndex);
            shader.SetBuffer(kernel, "BiomeBuffer", biomeBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer, biomeBuffer };
        }
    }
}