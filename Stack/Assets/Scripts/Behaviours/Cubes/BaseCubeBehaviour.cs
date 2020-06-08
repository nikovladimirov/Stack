using UnityEngine;

namespace Behaviours
{
    public class BaseCubeBehaviour : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            AwakeImpl();
        }

        public virtual void Init(Vector3 position, Vector3 size, Color color)
        {
            transform.position = position;
            transform.localScale = size;
            Color = color;
        }
        
        protected  virtual void AwakeImpl()
        {
        }

        public Vector3 Position
        {
            get { return transform.position;}
            set { transform.position = value; }
        }
        public Vector3 Scale
        {
            get { return transform.localScale;}
            set { transform.localScale = value; }
        }
        
        public Color Color
        {
            get { return _meshRenderer.material.color; }
            protected set { _meshRenderer.material.color = value; }
        }
    }
}