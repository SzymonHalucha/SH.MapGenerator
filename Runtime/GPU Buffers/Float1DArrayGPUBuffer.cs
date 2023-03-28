using UnityEngine;
using Unity.Collections;

namespace SH.MapGenerator.GPUBuffers
{
    [CreateAssetMenu(menuName = "SH/Map Generator/GPU Buffers/Float 1D Array", fileName = "New Float 1D Array Buffer", order = 0)]
    public class Float1DArrayGPUBuffer : BaseGPUBuffer
    {
        public override void Init(int size)
        {
            Size = size;
            Buffer = new ComputeBuffer(Size, sizeof(float));
            Buffer.SetData(new float[Size]);
        }

        public void Init(NativeArray<float> array)
        {
            Size = array.Length;
            Buffer = new ComputeBuffer(Size, sizeof(float));
            Buffer.SetData(array);
        }

        public void Init(float[] array)
        {
            Size = array.Length;
            Buffer = new ComputeBuffer(Size, sizeof(float));
            Buffer.SetData(array);
        }

        public override void Dispose()
        {
            Size = 0;
            Buffer.Release();
            Buffer = null;
        }

        public float[] GetData()
        {
            float[] data = new float[Size];
            Buffer.GetData(data);
            return data;
        }
    }
}