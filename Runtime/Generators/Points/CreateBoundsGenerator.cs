using System.Linq;
using UnityEngine;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;
using Bounds = SH.MapGenerator.Utils.Bounds;

namespace SH.MapGenerator.Generators.Points
{
    public class CreateBoundsGenerator : BaseGenerator
    {
        [SerializeField] private Vector1DArrayGPUBuffer pointsBuffer = null;
        [SerializeField] private Bounds1DArrayCPUBuffer boundsBuffer = null;
        [SerializeField] private int maskIndex = 0;

        public override void Generate(RuntimeData data)
        {
            Vector3[] points = pointsBuffer.GetData();
            int offset = data.HeightMapSize / Mathf.FloorToInt(Mathf.Sqrt(pointsBuffer.Size));
            Vector3[] selected = points.Where(p => p.z == maskIndex).ToArray();
            Bounds[] bounds = new Bounds[selected.Length];

            for (int i = 0; i < bounds.Length; i++)
            {
                Vector2 min = new Vector2(selected[i].x * data.HeightMapSize - offset, selected[i].y * data.HeightMapSize - offset);
                Vector2 max = new Vector2(selected[i].x * data.HeightMapSize + offset, selected[i].y * data.HeightMapSize + offset);
                bounds[i] = new Bounds(min, max);
            }

            boundsBuffer.Init(bounds);
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { pointsBuffer };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { boundsBuffer };
        }
    }
}