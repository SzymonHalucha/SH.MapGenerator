using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SH.MapGenerator.Utils
{
    public static class ComputeShadersContrainer
    {
        private static Dictionary<string, ComputeShader> cachedKernels;

        private static void Init()
        {
            cachedKernels = Resources.LoadAll<ComputeShader>("Map Generator").ToDictionary(shader => shader.name);
        }

        public static ComputeShader GetShader(string name)
        {
            if (cachedKernels == null)
                Init();

            return cachedKernels[name];
        }
    }
}