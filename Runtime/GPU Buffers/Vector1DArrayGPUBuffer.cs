using UnityEngine;
using Unity.Collections;

namespace SH.MapGenerator.GPUBuffers
{
    [CreateAssetMenu(menuName = "SH/Map Generator/GPU Buffers/Vector 1D Array", fileName = "New Vector 1D Array Buffer", order = 0)]
    public class Vector1DArrayGPUBuffer : BaseGPUBuffer
    {
        public override void Init(int size)
        {
            Size = size;
            Buffer = new ComputeBuffer(Size, sizeof(float) * 2);
            Buffer.SetData(new Vector2[Size]);
        }

        public void Init(int size, NativeArray<Vector2> array)
        {
            Size = size;
            Buffer = new ComputeBuffer(Size, sizeof(float) * 2);
            Buffer.SetData(array);
        }

        public void Init(int size, Vector2[] array)
        {
            Size = size;
            Buffer = new ComputeBuffer(Size, sizeof(float) * 2);
            Buffer.SetData(array);
        }

        public override void DeInit()
        {
            Size = 0;
            Buffer.Release();
            Buffer = null;
        }

        public Vector2[] GetData()
        {
            Vector2[] data = new Vector2[Size];
            Buffer.GetData(data);
            return data;
        }
    }
}