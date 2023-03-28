using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SH.MapGenerator.Sources;
using SH.MapGenerator.GPUBuffers;
using SH.MapGenerator.CPUBuffers;

namespace SH.MapGenerator.Generators.Points
{
    public class SetPrefabsGenerator : BaseGenerator
    {
        [System.Serializable]
        private struct PrefabContainer
        {
            [SerializeField] private BaseSource prefab;
            [SerializeField, Range(0, 1f)] private float weight;
            [SerializeField, Range(0, 16f)] private float radius;
            [SerializeField] private bool destroyOthers;
            [SerializeField] private bool makeParent;

            public BaseSource Prefab => prefab;
            public float Weight => weight;
            public float Radius => radius;
            public bool DestroyOthers => destroyOthers;
            public bool MakeParent => makeParent;
        }

        [SerializeField] private Vector1DArrayCPUBuffer pointsBuffer = null;
        [SerializeField] private SourcesManager sourcesManager = null;
        [SerializeField] private LayerMask resourceLayer = 0;
        [SerializeField] private PrefabContainer[] containers = new PrefabContainer[0];

        public override void Generate(RuntimeData data)
        {
            int amount = 0;
            int[] indexes = new int[pointsBuffer.Size];
            float[] weights = containers.Select(x => x.Weight).ToArray();

            for (int i = 0; i < pointsBuffer.Size; i++)
                indexes[i] = data.Random.Weighted(weights);

            GameObject[] parents = new GameObject[containers.Length];
            Collider[] colliders = new Collider[32];

            for (int i = 0; i < containers.Length; i++)
            {
                for (int j = 0; j < pointsBuffer.Size; j++)
                {
                    if (pointsBuffer.Vectors[j].z <= 0 || indexes[j] != i)
                        continue;

                    Vector3 position = new Vector3(pointsBuffer.Vectors[j].x * data.HeightMapSize, pointsBuffer.Vectors[j].z * data.HeightMapScale, pointsBuffer.Vectors[j].y * data.HeightMapSize);
                    Quaternion rotation = Quaternion.Euler(0f, data.Random.Range(360f), 0f);
                    PrefabContainer selected = containers[indexes[j]];

                    if (selected.MakeParent && parents[indexes[j]] == null)
                        parents[indexes[j]] = new GameObject($"{selected.Prefab.name}s Parent");

                    if (selected.DestroyOthers)
                        amount = Physics.OverlapSphereNonAlloc(position, selected.Radius, colliders, resourceLayer);

                    Transform parent = selected.MakeParent ? parents[indexes[j]].transform : null;
                    GameObject.Instantiate(selected.Prefab, position, rotation, parent).Init(sourcesManager);

                    if (selected.DestroyOthers && amount > 0)
                    {
                        for (int k = 0; k < amount; k++)
                        {
                            colliders[k].GetComponent<BaseSource>().DeInit();

                            if (Application.isPlaying)
                                GameObject.Destroy(colliders[k].gameObject);
                            else
                                GameObject.DestroyImmediate(colliders[k].gameObject);
                        }
                    }
                }
            }
        }

        public override BaseGPUBuffer[] GetAllGPUBuffers()
        {
            return new BaseGPUBuffer[] { };
        }

        public override BaseCPUBuffer[] GetAllCPUBuffers()
        {
            return new BaseCPUBuffer[] { pointsBuffer };
        }
    }
}