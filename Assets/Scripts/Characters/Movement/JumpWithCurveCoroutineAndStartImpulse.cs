using System;
using System.Collections;
using UnityEngine;

namespace Characters.Movement
{
    public class JumpWithCurveCoroutineAndStartImpulse : JumpWithCurveCoroutine
    {
        [SerializeField] protected ImpulseForceConfig impulseForceConfig;

        protected override IEnumerator JumpCoroutine()
        {
            yield return new WaitForFixedUpdate();
            Rigidbody.AddForce(transform.up * impulseForceConfig.impulseForce, ForceMode2D.Impulse);
            yield return base.JumpCoroutine();
        }

        [Serializable]
        protected class ImpulseForceConfig
        {
            public float impulseForce;
        }
    }
}