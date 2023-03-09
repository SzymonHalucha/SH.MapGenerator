using UnityEngine;

namespace SH.MapGenerator
{
    [CreateAssetMenu(menuName = "SH/Map Generator/Map Generator Data", fileName = "New Map Generator Data", order = 3)]
    public class MapGeneratorData : ScriptableObject
    {
        private enum Resolution : int
        {
            R32 = 32,
            R64 = 64,
            R128 = 128,
            R256 = 256,
            R512 = 512,
            R1024 = 1024,
            R2048 = 2048,
            R4096 = 4096
        }

        [Header("Map")]
        [SerializeField, Range(1f, 512f)] private float maxHeight = 128f;
        [SerializeField] private Resolution heightMapSize = Resolution.R512;
        [SerializeField] private Resolution splatMapSize = Resolution.R512;

        [Header("Noises")]
        [Space, SerializeField] private NoiseData[] noises = new NoiseData[0];

        public float MaxHeight => maxHeight;
        public int HeightMapSize => (int)heightMapSize;
        public int SplatMapSize => (int)splatMapSize;
        public NoiseData[] Noises => noises;
    }
}