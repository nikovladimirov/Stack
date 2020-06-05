using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviours
{
    public class CubeBehaviour : MonoBehaviour
    {
        private bool _isDropped;
        private bool _isCutted;
        private bool _osX;
        private bool _directionPlus = true;
        private MeshRenderer _meshRenderer;

        private const float _speed = 8f;
        private const int _value = 10;

        public float MinY { get; private set; }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public Color Color
        {
            get
            {
                return _meshRenderer.material.color;
            }
            set
            {
                _meshRenderer.material.color = value;
            }
        }

        public void Drop()
        {
            if (_isDropped)
                return;
            _isDropped = true;

            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isDropped || _isCutted)
                return;

            _isCutted = true;

            var bounds = new Bounds();
            var points = collision.contacts.Select(x => x.point).ToList();

            foreach (var vector3 in points)
                bounds.Encapsulate(vector3);

            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;

            transform.position = new Vector3(bounds.center.x, transform.position.y, bounds.center.z);
            transform.localScale = new Vector3(bounds.size.x, transform.localScale.y, bounds.size.z);

            GameManager.Instance.SpawnNextCube();
        }

        public void Init(CubeBehaviour prevCube)
        {
            if (prevCube == null)
            {
                _isCutted = true;
                _isDropped = true;
                return;
            }

            var prevCubeTransform = prevCube.transform;
            var osX = Random.Range(0, 2);
            if (osX == 1)
            {
                _osX = true;
                transform.position = new Vector3(GetInitValue(),
                    prevCubeTransform.position.y + prevCubeTransform.localScale.y, 0);
            }
            else
            {
                _osX = false;
                transform.position = new Vector3(0, prevCubeTransform.position.y + prevCubeTransform.localScale.y,
                    GetInitValue());
            }

            transform.localScale = prevCubeTransform.localScale;
            MinY = transform.position.y - transform.localScale.y / 2;
        }

        private float GetInitValue()
        {
            return Random.Range(0, 2) == 1 ? _value : -_value;
        }


        private void FixedUpdate()
        {
            if (_isDropped)
            {
                if (!_isCutted && transform.position.y < MinY)
                    GameManager.Instance.SetGameState(GameState.Death);
                return;
            }

            if (_osX)
            {
                transform.position = new Vector3(GetNewPosition(transform.position.x), transform.position.y,
                    transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y,
                    GetNewPosition(transform.position.z));
            }
        }

        private float GetNewPosition(float position)
        {
            var newValue = Mathf.Clamp(_directionPlus
                ? position + Time.deltaTime * _speed
                : position - Time.deltaTime * _speed, -_value, _value);

            if (newValue == _value || newValue == -_value)
                _directionPlus = !_directionPlus;

            return newValue;
        }
    }
}