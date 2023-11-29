using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace World.Runtime
{
    public class Door : NetworkBehaviour
    {
        [Header("Setup")]
        [SerializeField] private float openDuration = 1;
        [SerializeField] private Vector3 openOffset = Vector3.up;
        
        [Header("Animator")]
        [SerializeField] private Animator animator;
        [SerializeField] private string openParam = "isOpen";
        
        private Action<Door> onOpen;

        public int PlatesNeeded { get; private set; }
        [field: SerializeField] public List<PressurePlate> PressurePlates { get; set; }

        public void AddPressurePlate(PressurePlate pressurePlate)
        {
            pressurePlate.OnPress += HandlePress;
            PlatesNeeded++;
        }

        private void HandlePress(PressurePlate pressurePlate)
        {
            PlatesNeeded--;
            pressurePlate.OnPress -= HandlePress;
            if (PlatesNeeded <= 0)
                StartCoroutine(Open());
        }

        private IEnumerator Open()
        {
            Debug.Log($"[{name}] was opened!");
            if (animator)
                animator.SetBool(openParam, true);
            var startPosition = transform.position;
            var destPosition = startPosition + openOffset;
            var startTime = Time.time;
            var now = Time.time;
            while (startTime + openDuration > now)
            {
                var lerp = (now - startTime) / openDuration;
                transform.position = Vector3.Lerp(startPosition, destPosition, lerp);
                yield return null;
                now = Time.time;
            }

            transform.position = destPosition;
            onOpen?.Invoke(this);
        }
    }
}