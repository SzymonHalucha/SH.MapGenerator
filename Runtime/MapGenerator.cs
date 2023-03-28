using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using SH.MapGenerator.Sources;
using SH.MapGenerator.Generators;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator
{
    [CreateAssetMenu(menuName = "SH/Map Generator/Map Generator", fileName = "New Map Generator", order = 3)]
    public class MapGenerator : ScriptableObject
    {
        [Header("Map")]
        [SerializeField, Range(1f, 512f)] private int scale = 128;
        [SerializeField] private MapResolution heightMapSize = MapResolution.R512;
        [SerializeField] private MapResolution splatMapSize = MapResolution.R512;

        [Space, SerializeField] private NoiseData[] noises = new NoiseData[0];

        public IEnumerable<float> RunPipeline(int seed)
        {
            int totalLength = noises.Sum(noise => noise.Generators.Length);
            RuntimeData runtimeData = InitializePipeline(seed);
            InitializeAllGPUBuffers(runtimeData);

            int index = 0;
            foreach (NoiseData noise in noises)
                if (noise.Enabled)
                    foreach (BaseGenerator generator in noise.Generators)
                        if (generator.Enabled)
                        {
                            generator.Generate(runtimeData);
                            yield return (index++) / (float)totalLength;
                        }

            DeinitializePipeline(runtimeData);
            yield return 1f;
        }

        private RuntimeData InitializePipeline(int seed)
        {
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SetLeakDetectionMode(Unity.Collections.NativeLeakDetectionMode.EnabledWithStackTrace);
            RuntimeData runtimeData = new RuntimeData
            {
                GPUBuffers = FindAllUsedGPUBuffers(),
                CPUBuffers = FindAllUsedCPUBuffers(),
                TerrainLayers = FindAllTerrainLayers(),
                Random = new SH.MapGenerator.Utils.Random(seed),
                HeightMapScale = scale,
                HeightMapSize = (int)heightMapSize,
                SplatMapSize = (int)splatMapSize
            };

            return runtimeData;
        }

        private void DeinitializePipeline(RuntimeData data)
        {
            foreach (BaseGPUBuffer buffer in data.GPUBuffers)
                buffer.Dispose();

            foreach (BaseCPUBuffer buffer in data.CPUBuffers)
                buffer.Dispose();
        }

        private HashSet<BaseGPUBuffer> FindAllUsedGPUBuffers()
        {
            HashSet<BaseGPUBuffer> gpuHashSet = new HashSet<BaseGPUBuffer>();

            foreach (NoiseData noise in noises)
                if (noise.Enabled)
                    SearchForGPUBuffersInGenerator(noise.Generators, gpuHashSet);

            return gpuHashSet;
        }

        private void SearchForGPUBuffersInGenerator(BaseGenerator[] generators, HashSet<BaseGPUBuffer> gpuHashSet)
        {
            foreach (BaseGenerator generator in generators)
            {
                BaseGPUBuffer[] gpuBuffers = generator.GetAllGPUBuffers();
                foreach (BaseGPUBuffer buffer in gpuBuffers)
                    if (buffer != null)
                        gpuHashSet.Add(buffer);
            }
        }

        private HashSet<BaseCPUBuffer> FindAllUsedCPUBuffers()
        {
            HashSet<BaseCPUBuffer> cpuHashSet = new HashSet<BaseCPUBuffer>();

            foreach (NoiseData noise in noises)
                if (noise.Enabled)
                    SearchForCPUBuffersInGenerator(noise.Generators, cpuHashSet);

            return cpuHashSet;
        }

        private void SearchForCPUBuffersInGenerator(BaseGenerator[] generators, HashSet<BaseCPUBuffer> cpuHashSet)
        {
            foreach (BaseGenerator generator in generators)
            {
                BaseCPUBuffer[] cpuBuffers = generator.GetAllCPUBuffers();
                foreach (BaseCPUBuffer buffer in cpuBuffers)
                    if (buffer != null)
                        cpuHashSet.Add(buffer);
            }
        }

        private Dictionary<TerrainLayer, int> FindAllTerrainLayers()
        {
            Dictionary<TerrainLayer, int> dictionary = new Dictionary<TerrainLayer, int>();

            int index = -1;
            foreach (NoiseData noise in noises)
                if (noise.Enabled)
                    foreach (BaseGenerator generator in noise.Generators)
                        if (generator.Enabled)
                            foreach (TerrainLayer layer in generator.GetAllTerrainLayers())
                                if (!dictionary.ContainsKey(layer))
                                    dictionary.Add(layer, ++index);

            return dictionary;
        }

        private void InitializeAllGPUBuffers(RuntimeData data)
        {
            using (NativeArray<float> array = new NativeArray<float>((int)heightMapSize * (int)heightMapSize, Allocator.Temp))
                foreach (BaseGPUBuffer buffer in data.GPUBuffers)
                    if (buffer is Float2DArrayGPUBuffer float2DArray)
                        float2DArray.Init((int)heightMapSize, (int)heightMapSize, array);

            using (NativeArray<float> array = new NativeArray<float>((int)splatMapSize * (int)splatMapSize * data.TerrainLayers.Count, Allocator.Temp))
                foreach (BaseGPUBuffer buffer in data.GPUBuffers)
                    if (buffer is Float3DArrayGPUBuffer float3DArray)
                        float3DArray.Init((int)splatMapSize, (int)splatMapSize, data.TerrainLayers.Count, array);
        }
    }
}