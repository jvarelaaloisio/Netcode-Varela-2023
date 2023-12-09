using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

namespace Player
{
    public class PlayerInputReader : NetworkBehaviour
    {
        [field: SerializeField] public Characters.Control.CharacterController CharacterController { get; private set; }
        [Header("Input Actions")]
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private string runActionName = "Run";
        [SerializeField] private string jumpActionName = "Jump";
    
        private PlayerInput _playerInput;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
                StartCoroutine(SubscribeToInputAfterOneFrame());
            else
            {
                Debug.LogWarning($"{name}: disabling component since it's not owned by this client.");
                enabled = false;
            }
        
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!inputActionAsset)
            {
                Debug.LogError($"{name} ({GetType().Name}): {nameof(inputActionAsset)} is null!");
                return;
            }
            var runAction = inputActionAsset.FindAction(runActionName);
            if (runAction != null)
            {
                runAction.performed -= HandleRunInput;
                runAction.canceled -= HandleRunInput;
            }

            var jumpAction = inputActionAsset.FindAction(jumpActionName);
            if (jumpAction != null)
            {
                jumpAction.started -= HandleJumpInputStarted;
                jumpAction.canceled -= HandleJumpInputCanceled;
            }
        }

        /// <summary>
        /// Subscribes to all read inputs after one frame, to avoid any lag-related high/wrong values.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SubscribeToInputAfterOneFrame()
        {
            if (!inputActionAsset)
            {
                Debug.LogError($"{name} ({GetType().Name}): {nameof(inputActionAsset)} is null!");
                yield break;
            }

            yield return null;
            var runAction = inputActionAsset.FindAction(runActionName);
            if (runAction != null)
            {
                runAction.performed += HandleRunInput;
                runAction.canceled += HandleRunInput;
            }

            var jumpAction = inputActionAsset.FindAction(jumpActionName);
            if (jumpAction != null)
            {
                jumpAction.started += HandleJumpInputStarted;
                jumpAction.canceled += HandleJumpInputCanceled;
            }
        }

        private void HandleJumpInputCanceled(InputAction.CallbackContext obj)
        {
            CharacterController.CancelJump();
        }

        private void HandleRunInput(InputAction.CallbackContext obj)
        {
            var inputValue = obj.ReadValue<Vector2>();
            CharacterController.SetRunDirection(inputValue.x);
        }

        private void HandleJumpInputStarted(InputAction.CallbackContext obj)
        {
            if (!CharacterController.TryBeginJump(out var failReason))
            {
                Debug.Log($"{name} ({GetType().Name}): Couldn't jump!" +
                          $"\nReason: {failReason}");
            }
        }
    }
}
