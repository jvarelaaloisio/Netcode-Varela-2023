using Unity.Netcode;
using UnityEngine;

namespace World.Runtime
{
    public class PressurePlateAnimatorController : NetworkBehaviour
    {
        [SerializeField] private PressurePlate pressurePlate;
        [SerializeField] private Animator animator;
        [SerializeField] private string pressedParam;
        private int _pressedParamHash = 0;
        
        private void Awake()
        {
            _pressedParamHash = Animator.StringToHash(pressedParam);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsHost)
            {
                Destroy(this);
            }
            pressurePlate.OnPress += HandlePress;
            pressurePlate.OnReset += HandleReset;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            pressurePlate.OnPress -= HandlePress;
            pressurePlate.OnReset -= HandleReset;
        }

        private void HandlePress(PressurePlate plate)
        {
            animator.SetBool(_pressedParamHash, true);
        }

        private void HandleReset(PressurePlate plate)
        {
            animator.SetBool(_pressedParamHash, false);
        }
    }
}