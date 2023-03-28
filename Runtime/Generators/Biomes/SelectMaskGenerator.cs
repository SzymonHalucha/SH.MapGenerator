using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Biomes
{
    public class SelectMaskGenerator : BaseGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer maskBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField] private int maskIndex = 0;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("SelectMask");
            int kernel = shader.FindKernel("SelectMask");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_Index", maskIndex);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);
            shader.SetBuffer(kernel, "MaskBuffer", maskBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { targetBuffer, maskBuffer };
        }
    }
}