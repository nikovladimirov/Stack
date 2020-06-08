using UnityEngine;

namespace Behaviours
{
    public class TrashCubeBehaviour : BaseCubeBehaviour
    {
        private float _initY;
        private Renderer _renderer;

        protected override void AwakeImpl()
        {
            base.AwakeImpl();
            _renderer = GetComponent<Renderer>();
        }

        public override void Init(Vector3 position, Vector3 size, Color color)
        {
            base.Init(position, size, color);
            
            _initY = transform.position.y;
        }

        private void Update()
        {
            if (transform.position.y < _initY && !_renderer.isVisible)
                Destroy(gameObject);
        }
    }
}