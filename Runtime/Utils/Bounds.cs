using UnityEngine;

namespace SH.MapGenerator.Utils
{
    public class Bounds
    {
        public readonly Vector2 StartOffset;
        public readonly Vector2 EndOffset;
        public readonly int Size;

        public Bounds(Vector2 startOffset, Vector2 endOffset)
        {
            StartOffset = startOffset;
            EndOffset = endOffset;
            Size = Mathf.CeilToInt(Mathf.Max(EndOffset.x - StartOffset.x, EndOffset.y - StartOffset.y));
        }
    }
}