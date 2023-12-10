using System;
using EventChannels.Runtime.Additions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace World.Runtime
{
    public class PressurePlate : NetworkBehaviour
    {
        [SerializeField] private double maxAngle = 45;

        [Header("Colliders")]
        [SerializeField] private DelegateOnCollision2D openCollider;
        [SerializeField] private GameObject closedCollider;
        
        [Header("Animator")]
        [SerializeField] private Animator animator;
        [SerializeField] private string pressedParam;

        [Header("Events")]
        [SerializeField] private UnityEvent onPress;
        [SerializeField] private UnityEvent onReset;

        public event Action<PressurePlate> OnPress = delegate { };
        public event Action<PressurePlate> OnReset = delegate { };

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (openCollider != null)
                openCollider.onCollisionEnter2D += HandleOpenCollisionEnter2D;
            else
                Debug.LogWarning($"{name}: {nameof(openCollider)} is null!");
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (openCollider != null)
                openCollider.onCollisionEnter2D -= HandleOpenCollisionEnter2D;
        }

        private void HandleOpenCollisionEnter2D(Collision2D collision)
        {
            var contact = collision.contacts[0];
            Debug.DrawRay(contact.point, contact.normal, Color.red, 1);
            if (collision.transform.position.y > transform.position.y
                && Vector2.Angle(-contact.normal, transform.up) <= maxAngle)
                PressClientRpc();
        }

        [ClientRpc]
        private void PressClientRpc()
        {
            openCollider.gameObject.SetActive(false);
            if (closedCollider)
                closedCollider.SetActive(true);
            if (animator)
                animator.SetBool(pressedParam, true);
            OnPress(this);
            onPress.Invoke();
        }

        [ContextMenu("Reset to Default")]
        [ClientRpc]
        public void ResetToDefaultClientRpc()
        {
            openCollider.gameObject.SetActive(true);
            if (closedCollider)
                closedCollider.SetActive(false);
            if (animator)
                animator.SetBool(pressedParam, false);
            OnReset(this);
            onReset.Invoke();
        }
    }
}
