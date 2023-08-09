using System;
using System.Collections;
using UnityEngine;

namespace Characters.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpWithConstantForce : Jump
    {
        [SerializeField] protected ConstantForceConfig constantForceConfig;

        protected override IEnumerator JumpCoroutine()
        {
            float startTime = Time.time;
            while (Time.time < startTime + constantForceConfig.maxDuration)
            {
                yield return new WaitForFixedUpdate();
                if (Mathf.Abs(Rigidbody.velocity.y) < constantForceConfig.maxSpeed)
                {
                    Rigidbody.AddForce(transform.up * (constantForceConfig.force * Time.fixedDeltaTime), ForceMode2D.Force);
                }
            }
        }

        [Serializable]
        protected class ConstantForceConfig
        {
            public float force = 2000f;
            public float maxSpeed = 1f;
            public float maxDuration = .5f;
        }
    }
}