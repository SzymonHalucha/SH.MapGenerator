using UnityEngine;
using SH.MapGenerator.Sources;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Generators
{
    public class BuildSourcesTreesGenerator : BaseGenerator
    {
        [SerializeField] private SourcesManager sourcesManager = null;

        public override void Generate(RuntimeData data)
        {
            sourcesManager.BuildAllTrees();
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { };
        }
    }
}