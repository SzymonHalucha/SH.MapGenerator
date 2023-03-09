using UnityEngine;
using SH.MapGenerator.GPUBuffers;

namespace SH.MapGenerator.Generators.Modifiers
{
    public abstract class BaseModifierGenerator : BaseGenerator
    {
        [SerializeField] protected Float2DArrayGPUBuffer TargetBuffer = null;

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { TargetBuffer };
        }
    }
}