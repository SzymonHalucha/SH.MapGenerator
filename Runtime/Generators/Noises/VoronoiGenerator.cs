using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Noises
{
    public class VoronoiGenerator : BaseGenerator
    {
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer targetBuffer = null;

        public override void Generate(RuntimeData data)
        {
            if (boundsBuffer != null && boundsBuffer.Size == 0)
                return;

            ComputeShader shader = ComputeShadersContrainer.GetShader("Voronoi");
            int kernel = shader.FindKernel("Voronoi");

            shader.SetInt("_Size", targetBuffer.Width);
            shader.SetInt("_PointsCount", pointsBuffer.Size);
            shader.SetVector("_StartOffset", boundsBuffer != null ? boundsBuffer.StartOffset : Vector2.zero);
            shader.SetBuffer(kernel, "PointsBuffer", pointsBuffer.Buffer);
            shader.SetBuffer(kernel, "TargetBuffer", targetBuffer.Buffer);

            if (boundsBuffer != null)
                DispatchComputeShader(shader, kernel, boundsBuffer.Size, boundsBuffer.Size);
            else
                DispatchComputeShader(shader, kernel, targetBuffer.Width, targetBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer, targetBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { boundsBuffer };
        }
    }
}