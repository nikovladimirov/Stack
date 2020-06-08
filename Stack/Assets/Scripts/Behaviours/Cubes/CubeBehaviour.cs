using System;
using DefaultNamespace;
using DefaultNamespace.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviours
{
    public class CubeBehaviour : BaseCubeBehaviour
    {
        private bool _isDropped;
        private bool _isStatic;
        private bool _directionPlus;
        private float _minY;
        private Direction _direction;
        
        private MeshRenderer _meshRenderer;
        private CubeBehaviour _prevCube;
        private bool _withScore;
        private float _startPosition;

        public CubeBehaviour WithScore
        {
            get
            {
                if (_withScore)
                    return this;
                
                return _prevCube?.WithScore;
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

        protected override void AwakeImpl()
        {
            base.AwakeImpl();
            _startPosition = GameManager.Instance.StartPosition;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isDropped || _isStatic)
                return;

            _isStatic = true;
            _withScore = true;

            var t = transform;
            var otherT = collision.transform;

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
            // Handheld.Vibrate();  //TODO: легкий taptic engine.
            GameManager.Instance.SpawnNextCube();
            _isStatic = true;
        }

        public override void Init(Vector3 position, Vector3 size, Color color)
        {
            base.Init(position, size, color);
            
            _isDropped = true;
            _isStatic = true;
            _withScore = true;
        }
        
        public void Init(CubeBehaviour prevCube, Color color)
        {
            Color = color;
            if (prevCube == null)
            {
                _isStatic = true;
                _isDropped = true;
                _withScore = true;
                return;
            }

            _prevCube = prevCube;

            var prevCubeTransform = prevCube.transform;
            _direction = Random.Range(0, 2) == 0 ? Direction.X : Direction.Z;
            if (_direction == Direction.X)
            {
                transform.position = new Vector3(
                    GetInitValue(),
                    prevCubeTransform.position.y + prevCubeTransform.localScale.y,
                    prevCubeTransform.position.z
                );
            }
            else
            {
                transform.position = new Vector3(
                    prevCubeTransform.position.x,
                    prevCubeTransform.position.y + prevCubeTransform.localScale.y,
                    GetInitValue()
                );
            }

            transform.localScale = prevCubeTransform.localScale;
            _minY = transform.position.y - transform.localScale.y / 2;
        }

        private float GetInitValue()
        {
            return Random.Range(0, 2) == 1 ? _startPosition: -_startPosition;
        }


        private void Update()
        {
            if (!_isDropped || _isStatic)
                return;

            if (transform.position.y < _minY)
            {
                _isStatic = true;
                GameManager.Instance.SetGameState(GameState.Death);
            }
        }

        private float GetNewPosition(float position)
        {
            var newValue = Mathf.Clamp(_directionPlus
                ? position + Time.deltaTime * GameManager.Instance.CubeSpeed
                : position - Time.deltaTime * GameManager.Instance.CubeSpeed, -_startPosition, _startPosition);

            if (newValue == _startPosition || newValue == -_startPosition)
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