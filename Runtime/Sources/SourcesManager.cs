using System;
using System.Collections.Generic;
using UnityEngine;

namespace SH.MapGenerator.Sources
{
    [CreateAssetMenu(menuName = "SH/Map Generator/Sources Manager", fileName = "New Sources Manager", order = 4)]
    public class SourcesManager : ScriptableObject
    {
        [System.NonSerialized] private Dictionary<Type, BaseSource> roots = new();
        [System.NonSerialized] private Dictionary<Type, List<BaseSource>> sources = new();

        public void AddSource(BaseSource source)
        {
            Type sourceType = source.GetType();
            if (!sources.ContainsKey(sourceType))
                sources.Add(sourceType, new List<BaseSource>());

            sources[sourceType].Add(source);
        }

        public void RemoveSource(BaseSource source)
        {
            Type sourceType = source.GetType();
            if (!sources.ContainsKey(sourceType))
                return;

            sources[sourceType].Remove(source);
        }

        public void BuildAllTrees()
        {
            roots.Clear();
            foreach (Type sourceType in sources.Keys)
                roots.Add(sourceType, BuildTree(sources[sourceType], sourceType, 0));

            sources.Clear();
        }

        public BaseSource FindClosest(Vector3 position, Type sourceType)
        {
            BaseSource closest = null;
            float minDistance = float.MaxValue;

            FindClosest(roots[sourceType], position, 0, ref closest, ref minDistance);
            return closest;
        }

        private BaseSource BuildTree(List<BaseSource> sources, Type sourceType, int depth)
        {
            if (sources == null || sources.Count == 0)
                return null;

            int axis = depth % 3;
            int median = sources.Count / 2;
            sources.Sort((a, b) => a.Position[axis].CompareTo(b.Position[axis]));

            BaseSource root = sources[median];
            root.SetLeft(BuildTree(sources.GetRange(0, median), sourceType, depth + 1));
            root.SetRight(BuildTree(sources.GetRange(median + 1, sources.Count - median - 1), sourceType, depth + 1));
            return root;
        }

        private void FindClosest(BaseSource source, Vector3 position, int depth, ref BaseSource closest, ref float minDistance)
        {
            if (source == null)
                return;

            float distance = (position - source.Position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = source;
            }

            int axis = depth % 3;
            if (position[axis] < source.Position[axis])
            {
                FindClosest(source.Left, position, depth + 1, ref closest, ref minDistance);
                if (Mathf.Abs(position[axis] - source.Position[axis]) < minDistance)
                    FindClosest(source.Right, position, depth + 1, ref closest, ref minDistance);
            }
            else
            {
                FindClosest(source.Right, position, depth + 1, ref closest, ref minDistance);
                if (Mathf.Abs(position[axis] - source.Position[axis]) < minDistance)
                    FindClosest(source.Left, position, depth + 1, ref closest, ref minDistance);
            }
        }
    }
}