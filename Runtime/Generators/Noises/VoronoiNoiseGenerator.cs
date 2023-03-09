using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Noises
{
    public class VoronoiGenerator : BaseNoiseGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("Voronoi");
            int kernel = shader.FindKernel("Voronoi");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_PointsCount", pointsBuffer.Size);
            shader.SetBuffer(kernel, "PointsBuffer", pointsBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer, targetBuffer };
        }
    }
}