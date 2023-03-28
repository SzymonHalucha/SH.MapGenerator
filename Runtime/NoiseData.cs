using UnityEngine;
using SH.MapGenerator.Generators;

namespace SH.MapGenerator
{
    [CreateAssetMenu(menuName = "SH/Map Generator/Noise Data", fileName = "New Noise Data", order = 2)]
    public class NoiseData : ScriptableObject
    {
        [SerializeField] private bool enabled = true;
        [SerializeReference] private BaseGenerator[] generators = new BaseGenerator[0];

        public bool Enabled => enabled;
        public BaseGenerator[] Generators => generators;
    }
}