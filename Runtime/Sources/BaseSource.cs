using UnityEngine;

namespace SH.MapGenerator.Sources
{
    public class BaseSource : MonoBehaviour
    {
        [SerializeField] protected Transform Transform = null;

        public BaseSource Left { get; private set; } = null;
        public BaseSource Right { get; private set; } = null;
        public SourcesManager Manager { get; private set; } = null;

        public Vector3 Position => Transform.position;

        public void Init(SourcesManager manager)
        {
            this.Manager = manager;
            this.Manager.AddSource(this);
            this.Transform = transform;
        }

        public void DeInit()
        {
            Left = null;
            Right = null;
            Manager.RemoveSource(this);
        }

        public void SetLeft(BaseSource source)
        {
            Left = source;
        }

        public void SetRight(BaseSource source)
        {
            Right = source;
        }
    }
}