using System.Collections.Generic;
using UnityEngine;

namespace SH.MapGenerator.Utils
{
    public static class ComputeShadersContrainer
    {
        private static Dictionary<string, ComputeShader> cachedKernels;
        
        private static void Init()
        {
            cachedKernels = new Dictionary<string, ComputeShader>();

            foreach (ComputeShader shader in Resources.LoadAll<ComputeShader>("Map Generator"))
                cachedKernels.Add(shader.name, shader);
        }

        public static ComputeShader GetShader(string name)
        {
            if (cachedKernels == null)
                Init();

            return cachedKernels[name];
        }
    }
}