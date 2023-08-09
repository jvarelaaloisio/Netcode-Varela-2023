using System;
using System.Collections;
using UnityEngine;

namespace Characters.Movement
{
    public abstract class Jump : MonoBehaviour
    {
        private float _lastJumpStart = 0f;
        protected Rigidbody2D Rigidbody;
        private Coroutine _currentCoroutine;
        
        
        public event Action OnBegan = delegate { };
        public event Action OnCanceled = delegate { };

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Starts applying a constant force to the character, until the jump is canceled or the max duration. 
        /// </summary>
        public void Begin()
        {
            var now = Time.time;

            _lastJumpStart = now;
            _currentCoroutine = StartCoroutine(JumpCoroutine());
            OnBegan();
            Debug.Log($"{name} ({GetType().Name}): Started at {now}");
        }

        protected abstract IEnumerator JumpCoroutine();

        /// <summary>
        /// Cancels the jump force being applied to the character, if it's jumping
        /// </summary>
        public void Cancel()
        {
            if (_currentCoroutine == null)
                return;
            StopCoroutine(_currentCoroutine);
            OnCanceled();
            Debug.Log($"{name} ({GetType().Name}): Canceled");
        }
    }
}