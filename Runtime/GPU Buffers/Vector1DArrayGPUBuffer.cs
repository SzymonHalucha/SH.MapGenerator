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
            Buffer = new ComputeBuffer(Size, sizeof(float) * 3);
            Buffer.SetData(new Vector3[Size]);
        }

        public void Init(NativeArray<Vector3> array)
        {
            Size = array.Length;
            Buffer = new ComputeBuffer(Size, sizeof(float) * 3);
            Buffer.SetData(array);
        }

        public void Init(Vector3[] array)
        {
            Size = array.Length;
            Buffer = new ComputeBuffer(Size, sizeof(float) * 3);
            Buffer.SetData(array);
        }

        public override void Dispose()
        {
            Size = 0;
            Buffer.Release();
            Buffer = null;
        }

        public Vector3[] GetData()
        {
            Vector3[] data = new Vector3[Size];
            Buffer.GetData(data);
            return data;
        }
    }
}