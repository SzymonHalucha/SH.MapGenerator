using UnityEngine;
using SH.MapGenerator.Generators;

namespace SH.MapGenerator
{
    [CreateAssetMenu(menuName = "SH/Map Generator/Noise Data", fileName = "New Noise Data", order = 2)]
    public class NoiseData : ScriptableObject
    {
        [SerializeReference] private BaseGenerator[] generators = new BaseGenerator[0];

        public BaseGenerator[] Generators => generators;

        public void GenerateNoise(RuntimeData data)
        {
            foreach (BaseGenerator generator in generators)
                if (generator.Enabled)
                    generator.Generate(data);
        }
    }
}