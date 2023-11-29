using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct TransformState
    {
        public Vector3 LocalPosition;
        public Vector3 LocalEulerRotation;
        public Vector3 LocalScale;

        public TransformState(Transform transform)
        {
            LocalPosition = transform.localPosition;
            LocalEulerRotation = transform.localEulerAngles;
            LocalScale = transform.localScale;
        }

        public void ApplyTo(Transform transform)
        {
            transform.localPosition = LocalPosition;
            transform.localEulerAngles = LocalEulerRotation;
            transform.localScale = LocalScale;
        }
    }

}
