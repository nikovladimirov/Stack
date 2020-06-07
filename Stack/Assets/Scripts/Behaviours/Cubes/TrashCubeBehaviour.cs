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

        private void Update()
        {
            if (transform.position.y < _initY && !_renderer.isVisible)
                Destroy(gameObject);
        }

        public void Init(Vector3 position, Vector3 size, Color color)
        {
            transform.position = position;
            transform.localScale = size;
            Color = color;
            _initY = transform.position.y;
        }
    }
}