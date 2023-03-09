using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Noises
{
    public class BlueNoiseGenerator : BaseNoiseGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField, Range(0, 128)] private int AmountSqrt = 4;

        public override void Generate(RuntimeData data)
        {
            pointsBuffer.Init(AmountSqrt * AmountSqrt);
            ComputeShader shader = ComputeShadersContrainer.GetShader("BlueNoise");
            int kernel = shader.FindKernel("BlueNoise");

            shader.SetInt("_Size", AmountSqrt);
            shader.SetInt("_Seed", data.Random.Range(int.MaxValue));
            shader.SetInt("_AmountSqrt", AmountSqrt);
            shader.SetBuffer(kernel, "TargetBuffer", pointsBuffer.Buffer);

            DispatchComputeShader(shader, kernel, AmountSqrt, AmountSqrt);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer };
        }
    }
}