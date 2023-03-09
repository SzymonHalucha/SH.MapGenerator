using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.Utils;

namespace SH.MapGenerator.Generators.Biomes
{
    public class PasteOnSplatMapBiomeGenerator : BaseBiomeGenerator
    {
        [SerializeField] private Float3DArrayGPUBuffer splatMapBuffer = null;
        [SerializeField] private Float2DArrayGPUBuffer layerBuffer = null;
        [SerializeField] private TerrainLayer terrainLayer = null;

        public override void Generate(RuntimeData data)
        {
            ComputeShader shader = ComputeShadersContrainer.GetShader("PasteOnSplatMap");
            int kernel = shader.FindKernel("PasteOnSplatMap");

            shader.SetInt("_Size", splatMapBuffer.Width);
            shader.SetInt("_Depth", data.TerrainLayers.Count);
            shader.SetInt("_CurrentDepth", data.TerrainLayers[terrainLayer]);
            shader.SetFloat("_Multiplier", layerBuffer.Width / (float)splatMapBuffer.Width);
            shader.SetBuffer(kernel, "SplatMapBuffer", splatMapBuffer.Buffer);
            shader.SetBuffer(kernel, "LayerBuffer", layerBuffer.Buffer);

            DispatchComputeShader(shader, kernel, splatMapBuffer.Width, splatMapBuffer.Height);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { splatMapBuffer, layerBuffer };
        }

        public override TerrainLayer[] GetAllTerrainLayers()
        {
            return new TerrainLayer[] { terrainLayer };
        }
    }
}