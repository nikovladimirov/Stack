using System;
using DefaultNamespace;
using DefaultNamespace.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviours
{
    //TODO: удалить обломки за камерой
    public class CubeBehaviour : MonoBehaviour
    {
        private bool _isDropped;
        private bool _isCutted;
        private bool _osX;
        private bool _directionPlus = true;
        private MeshRenderer _meshRenderer;
        private Direction _direction;
        private CubeBehaviour _prevCube;
        private Renderer _renderer;

        private const int _value = 5;

        public float MinY { get; private set; }

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

        public CubeBehaviour WithScore
        {
            get
            {
                if (_prevCube == null)
                    return null;
                if (_prevCube.transform.position.y < transform.position.y)
                    return this;
                return _prevCube.WithScore;
            }
        }

        public bool IsVisible => _renderer.isVisible;

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
            
            var t = transform;
            var otherT = collision.transform;
            if (otherT != _prevCube.transform)
            {
                GameManager.Instance.SetGameState(GameState.Death);
                return;
            }
            
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;

            
            Vector3 newPosition, newSize, trashPosition, trashSize;
            if (_direction == Direction.X)
            {
                newPosition = new Vector3((t.position.x + otherT.position.x) / 2, t.position.y, t.position.z);
                var newSizeX = t.localScale.x - Math.Abs(t.position.x - otherT.position.x);
                newSize = new Vector3(newSizeX, t.localScale.y, t.localScale.z);

                trashSize = new Vector3(t.localScale.x - newSize.x, t.localScale.y, t.localScale.z);
                var trashPositionX = t.position.x > otherT.position.x
                    ? newPosition.x + newSizeX / 2 + trashSize.x / 2
                    : newPosition.x - newSizeX / 2 - trashSize.x / 2;
                trashPosition = new Vector3(trashPositionX, t.position.y, t.position.z);
            }
            else
            {
                newPosition = new Vector3(t.position.x, t.position.y, (t.position.z + otherT.position.z) / 2);
                var newSizeZ = t.localScale.z - Math.Abs(t.position.z - otherT.position.z);
                newSize = new Vector3(t.localScale.x, t.localScale.y, newSizeZ);

                trashSize = new Vector3(t.localScale.x, t.localScale.y, t.localScale.z - newSize.z);
                var trashPositionZ = t.position.z > otherT.position.z
                    ? newPosition.z + newSizeZ / 2 + trashSize.z / 2
                    : newPosition.z - newSizeZ / 2 - trashSize.z / 2;
                trashPosition = new Vector3(t.position.x, t.position.y, trashPositionZ);
            }

            GameManager.Instance.SpawnTrash(trashPosition, trashSize, Color);

            transform.position = newPosition;
            transform.localScale = newSize;
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

            _prevCube = prevCube;

            var prevCubeTransform = prevCube.transform;
            _direction = Random.Range(0, 2) == 0 ? Direction.X : Direction.Z;
            if (_direction == Direction.X)
            {
                _osX = true;
                transform.position = new Vector3(
                    GetInitValue(),
                    prevCubeTransform.position.y + prevCubeTransform.localScale.y,
                    prevCubeTransform.position.z
                );
            }
            else
            {
                _osX = false;
                transform.position = new Vector3(
                    prevCubeTransform.position.x,
                    prevCubeTransform.position.y + prevCubeTransform.localScale.y,
                    GetInitValue()
                );
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
        }

        private float GetNewPosition(float position)
        {
            var newValue = Mathf.Clamp(_directionPlus
                ? position + Time.deltaTime * GameManager.Instance.CubeSpeed
                : position - Time.deltaTime * GameManager.Instance.CubeSpeed, -_value, _value);

            if (newValue == _value || newValue == -_value)
                _directionPlus = !_directionPlus;

            return newValue;
        }

        public void NextPosition()
        {
            if (_isDropped)
                return;

            if (_direction == Direction.X)
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
    }
}