using System.Linq;
using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Generators
{
    public class SetSplatMapGenerator : BaseGenerator
    {
        [SerializeField] private Float3DArrayGPUBuffer splatMapBuffer = null;

        public override void Generate(RuntimeData data)
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            terrainData.terrainLayers = data.TerrainLayers.Count > 0 ? data.TerrainLayers.Keys.ToArray() : terrainData.terrainLayers;

            terrainData.alphamapResolution = data.SplatMapSize;
            terrainData.SetAlphamaps(0, 0, splatMapBuffer.GetData());
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { splatMapBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { };
        }
    }
}