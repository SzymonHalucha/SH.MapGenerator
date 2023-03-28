using System.Linq;
using UnityEngine;
using Bounds = SH.MapGenerator.Utils.Bounds;

namespace SH.MapGenerator.CPUBuffers
{
    [CreateAssetMenu(menuName = "SH/Map Generator/CPU Buffers/Bounds 1D Array", fileName = "New Bounds 1D Array Buffer", order = 1)]
    public class Bounds1DArrayCPUBuffer : BaseCPUBuffer
    {
        public Bounds[] Bounds { get; private set; }
        public Vector2 StartOffset { get; private set; }
        public Vector2 EndOffset { get; private set; }

        public void Init(Bounds[] bounds)
        {
            Bounds = bounds;
            if (Bounds.Length == 0)
                return;

            StartOffset = new Vector2(Bounds.Min(b => b.StartOffset.x), Bounds.Min(b => b.StartOffset.y));
            EndOffset = new Vector2(Bounds.Max(b => b.EndOffset.x), Bounds.Max(b => b.EndOffset.y));
            Size = Mathf.CeilToInt(Mathf.Max(EndOffset.x - StartOffset.x, EndOffset.y - StartOffset.y));
        }

        public override void Dispose()
        {
            Size = 0;
            Bounds = null;
            StartOffset = Vector2.zero;
            EndOffset = Vector2.zero;
        }

        public override int GetAmountOfAllocatedBytes()
        {
            return Bounds.Length * (sizeof(float) * 4 + sizeof(int)) + sizeof(float) * 4;
        }
    }
}