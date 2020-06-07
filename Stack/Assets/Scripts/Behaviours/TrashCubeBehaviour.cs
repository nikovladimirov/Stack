using System;
using DefaultNamespace;
using DefaultNamespace.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviours
{
    public class TrashCubeBehaviour : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private Renderer _renderer;
        private float _initY;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _renderer = GetComponent<Renderer>();
        }

        public Color Color
        {
            get { return _meshRenderer.material.color; }
            set { _meshRenderer.material.color = value; }
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