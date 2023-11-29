using Characters.Movement;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Visuals
{
    public class CharacterAnimatorController : NetworkBehaviour
    {
        private const string SpeedXParameter = "VelX";
        private const string JumpParameter = "Jump";

        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private Animator animator;
        [SerializeField] private Jump jump;


        private readonly int speedXParameterHash = Animator.StringToHash(SpeedXParameter);
        private readonly int JumpTriggerHash = Animator.StringToHash(JumpParameter);

        private void Reset()
        {
            animator ??= GetComponent<Animator>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }
            base.OnNetworkSpawn();
            if (jump) jump.OnBegan += HandleJumpBegan;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (jump) jump.OnBegan -= HandleJumpBegan;
        }

        private void Update()
        {
            animator.SetFloat(speedXParameterHash, rigidbody2D.velocity.x);
        }

        private void HandleJumpBegan()
        {
            animator.SetTrigger(JumpTriggerHash);
        }
    }
}
