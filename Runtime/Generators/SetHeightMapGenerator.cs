using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Generators
{
    public class SetHeightMapGenerator : BaseGenerator
    {
        [SerializeField] private Float2DArrayGPUBuffer heightMapBuffer = null;

        public override void Generate(RuntimeData data)
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            terrainData.size = new Vector3(data.HeightMapSize, data.HeightMapScale, data.HeightMapSize);

            terrainData.heightmapResolution = data.HeightMapSize + 1;
            terrainData.SetHeights(0, 0, heightMapBuffer.GetData());
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { heightMapBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { };
        }
    }
}