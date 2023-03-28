using System.Collections.Generic;
using UnityEngine;
using SH.MapGenerator.Sources;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using Random = SH.MapGenerator.Utils.Random;

namespace SH.MapGenerator
{
    public class RuntimeData
    {
        public HashSet<BaseGPUBuffer> GPUBuffers;
        public HashSet<BaseCPUBuffer> CPUBuffers;
        public Dictionary<TerrainLayer, int> TerrainLayers;
        public Random Random;
        public int HeightMapScale;
        public int HeightMapSize;
        public int SplatMapSize;
    }
}