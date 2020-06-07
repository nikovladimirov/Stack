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

        protected  virtual void AwakeImpl()
        {
            
        }
        
        public Color Color
        {
            get { return _meshRenderer.material.color; }
            protected set { _meshRenderer.material.color = value; }
        }
    }
}