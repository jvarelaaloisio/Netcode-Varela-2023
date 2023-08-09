using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpWithCurveCoroutine : Jump
    {
        [SerializeField] protected CurvedForceConfig curvedForceConfig;

        protected override IEnumerator JumpCoroutine()
        {
            var startTime = Time.time;
            var now = startTime;
            while (now < startTime + curvedForceConfig.maxDuration)
            {
                yield return new WaitForFixedUpdate();
                if (Mathf.Abs(Rigidbody.velocity.y) < curvedForceConfig.maxSpeed)
                {
                    var delta = now - startTime;
                    var force = curvedForceConfig.force
                                * curvedForceConfig.forceMultiplierOverTime.Evaluate(delta) * Time.fixedDeltaTime;
                    Rigidbody.AddForce(transform.up * force,
                        ForceMode2D.Force);
                }

                now = Time.time;
            }
        }

        [Serializable]
        protected class CurvedForceConfig
        {
            public AnimationCurve forceMultiplierOverTime
                = AnimationCurve.EaseInOut(0, 1, 1, .5f);
            public float force = 2000f;
            public float maxSpeed = 1f;
            public float maxDuration = .5f;
        }
    }
}