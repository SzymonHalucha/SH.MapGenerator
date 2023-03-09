using UnityEngine;
using Unity.Collections;

namespace SH.MapGenerator.GPUBuffers
{
    [CreateAssetMenu(menuName = "SH/Map Generator/GPU Buffers/Float 3D Array", fileName = "New Float 3D Array Buffer", order = 0)]
    public class Float3DArrayGPUBuffer : BaseGPUBuffer
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }

        public override void Init(int size)
        {
            Size = size;
            Buffer = new ComputeBuffer(size, sizeof(float));
            Buffer.SetData(new float[size]);
        }

        public void Init(int size, NativeArray<float> array)
        {
            Size = size;
            Buffer = new ComputeBuffer(size, sizeof(float));
            Buffer.SetData(array);
        }

        public void Init(int width, int height, int depth, NativeArray<float> array)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Size = width * height * depth;
            Buffer = new ComputeBuffer(Size, sizeof(float));
            Buffer.SetData(array);
        }

        public override void DeInit()
        {
            Width = 0;
            Height = 0;
            Depth = 0;
            Size = 0;
            Buffer.Release();
            Buffer = null;
        }

        public float[,,] GetData()
        {
            float[,,] data = new float[Width, Height, Depth];
            Buffer.GetData(data);
            return data;
        }
    }
}