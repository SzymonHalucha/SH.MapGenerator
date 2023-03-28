using UnityEngine;
using Unity.Collections;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Points
{
    public class BlueNoiseGenerator : BaseGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField, Range(0, 2048)] private int amountSqrt = 4;

        public override void Generate(RuntimeData data)
        {
            using (NativeArray<Vector3> array = new NativeArray<Vector3>(amountSqrt * amountSqrt, Allocator.Temp))
                pointsBuffer.Init(array);

            ComputeShader shader = ComputeShadersContrainer.GetShader("BlueNoise");
            int kernel = shader.FindKernel("BlueNoise");

            shader.SetInt("_Size", amountSqrt);
            shader.SetInt("_Seed", data.Random.Range(int.MaxValue));
            shader.SetInt("_AmountSqrt", amountSqrt);
            shader.SetBuffer(kernel, "TargetBuffer", pointsBuffer.Buffer);

            DispatchComputeShader(shader, kernel, amountSqrt, amountSqrt);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer };
        }
    }
}