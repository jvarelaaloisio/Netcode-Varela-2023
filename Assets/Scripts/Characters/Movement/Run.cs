using System;
using System.Collections;
using UnityEngine;

namespace Characters.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Run : MonoBehaviour
    {
        [SerializeField] private Config config;


        private Rigidbody2D _rigidbody;
        private float _scaledDirection;

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() => StartCoroutine(Move());

        private void OnDisable() => StopCoroutine(Move());

        private IEnumerator Move()
        {
            while (enabled)
            {
                yield return new WaitForFixedUpdate();
                var force = config.movementForce * _scaledDirection * Time.fixedDeltaTime;
                if (force == 0)
                    continue;
                if (force < 0 && _rigidbody.velocity.x < -config.targetSpeed
                    || force > 0 && _rigidbody.velocity.x > config.targetSpeed)
                    continue;

                _rigidbody.AddForce(Vector2.right * force, ForceMode2D.Force);
            }
        }

        public void SetDirection(float scaledDirection) => _scaledDirection = scaledDirection;
        
        [Serializable]
        public struct Config
        {
            public float movementForce;
            public float targetSpeed;
        }
    }
}
