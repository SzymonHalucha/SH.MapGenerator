using System.Collections.Generic;
using UnityEngine;
using Random = SH.MapGenerator.Utils.Random;

namespace SH.MapGenerator
{
    public class RuntimeData
    {
        private Random random;
        private Dictionary<TerrainLayer, int> terrainLayers;

        public Random Random => random;
        public IReadOnlyDictionary<TerrainLayer, int> TerrainLayers => terrainLayers;

        public RuntimeData(Random random, Dictionary<TerrainLayer, int> terrainLayers)
        {
            this.random = random;
            this.terrainLayers = terrainLayers;
        }
    }
}