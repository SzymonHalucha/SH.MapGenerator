using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using SH.MapGenerator.Generators;
using SH.MapGenerator.GPUBuffers;
using Random = SH.MapGenerator.Utils.Random;

namespace SH.MapGenerator
{
    public class MapGeneratorBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Terrain terrain = null;
        [SerializeField] private MapGeneratorData data = null;

        [Header("Buffers")]
        [SerializeField] private Float2DArrayGPUBuffer heightMapBuffer = null;
        [SerializeField] private Float3DArrayGPUBuffer splatMapBuffer = null;

        private HashSet<BaseGPUBuffer> cachedBuffers;
        private RuntimeData runtimeData;

        public void GenerateTerrain(int seed)
        {
            Init(seed);
            var map = GenerateWholeMap();

            TerrainData terrainData = terrain.terrainData;
            terrainData.terrainLayers = runtimeData.TerrainLayers.Count > 0 ? runtimeData.TerrainLayers.Keys.ToArray() : terrainData.terrainLayers;
            terrainData.size = new Vector3(data.HeightMapSize, data.MaxHeight, data.HeightMapSize);
            DeInit();

            if (map.HeightMap != null)
            {
                terrainData.heightmapResolution = data.HeightMapSize + 1;
                terrainData.SetHeights(0, 0, map.HeightMap);
            }

            if (map.SplatMap != null)
            {
                terrainData.alphamapResolution = data.SplatMapSize;
                terrainData.SetAlphamaps(0, 0, map.SplatMap);
            }
        }

        private void Init(int seed)
        {
            Random random = new Random(seed);
            Dictionary<TerrainLayer, int> layers = FindAllTerrainLayers();
            runtimeData = new RuntimeData(random, layers);

            cachedBuffers = FindAllUsedBuffers();
            InitalizeAllBuffers();
        }

        private void DeInit()
        {
            foreach (BaseGPUBuffer buffer in cachedBuffers)
                buffer.DeInit();

            cachedBuffers = null;
            runtimeData = null;
        }

        private (float[,] HeightMap, float[,,] SplatMap) GenerateWholeMap()
        {
            foreach (NoiseData noise in data.Noises)
                noise.GenerateNoise(runtimeData);

            if (splatMapBuffer == null || splatMapBuffer.Size == 0)
                return (heightMapBuffer.GetData(), null);
            else
                return (heightMapBuffer.GetData(), splatMapBuffer.GetData());
        }

        private void InitalizeAllBuffers()
        {
            int heightMapSize = data.HeightMapSize;
            using (NativeArray<float> array = new NativeArray<float>(heightMapSize * heightMapSize, Allocator.Temp))
                foreach (BaseGPUBuffer buffer in cachedBuffers)
                    if (buffer is Float2DArrayGPUBuffer float2DArray)
                        float2DArray.Init(heightMapSize, heightMapSize, array);

            int splatMapSize = data.SplatMapSize;
            using (NativeArray<float> array = new NativeArray<float>(splatMapSize * splatMapSize * runtimeData.TerrainLayers.Count, Allocator.Temp))
                foreach (BaseGPUBuffer buffer in cachedBuffers)
                    if (buffer is Float3DArrayGPUBuffer float3DArray)
                        float3DArray.Init(splatMapSize, splatMapSize, runtimeData.TerrainLayers.Count, array);
        }

        private HashSet<BaseGPUBuffer> FindAllUsedBuffers()
        {
            HashSet<BaseGPUBuffer> hashSet = new HashSet<BaseGPUBuffer>();

            foreach (NoiseData noise in data.Noises)
                SearchInGenerators(noise.Generators, hashSet);

            return hashSet;
        }

        private void SearchInGenerators(BaseGenerator[] generators, HashSet<BaseGPUBuffer> hashSet)
        {
            BaseGPUBuffer[] buffers;
            foreach (BaseGenerator generator in generators)
            {
                buffers = generator.GetAllGPUBuffers();
                foreach (BaseGPUBuffer buffer in buffers)
                    if (buffer != null)
                        hashSet.Add(buffer);
            }
        }

        private Dictionary<TerrainLayer, int> FindAllTerrainLayers()
        {
            Dictionary<TerrainLayer, int> dictionary = new Dictionary<TerrainLayer, int>();

            int index = -1;
            foreach (NoiseData noise in data.Noises)
                foreach (BaseGenerator generator in noise.Generators)
                    if (generator.Enabled)
                        foreach (TerrainLayer layer in generator.GetAllTerrainLayers())
                            if (!dictionary.ContainsKey(layer))
                                dictionary.Add(layer, ++index);

            return dictionary;
        }
    }
}