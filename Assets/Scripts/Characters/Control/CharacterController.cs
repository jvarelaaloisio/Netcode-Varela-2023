using System;
using Characters.Movement;
using Core;
using UnityEngine;

namespace Characters.Control
{
    public class CharacterController : MonoBehaviour
    {
        private const string MaxJumpQuantityReached = "max Jump quantity reached";
        [field: SerializeField] public Jump jump { get; private set; }
        [field: SerializeField] public Run run { get; private set; }
        [field: SerializeField] public JumpCountController jumpCountController { get; private set; }
        [field: SerializeField] public JumpModel JumpModel { get; private set; }

        private void OnValidate()
        {
            jump ??= GetComponent<Jump>();
            run ??= GetComponent<Run>();
            jumpCountController ??= GetComponent<JumpCountController>();
        }

        /// <summary>
        /// Tries to begin the jump.
        /// </summary>
        /// <param name="failReason">Filled if the return value is false</param>
        /// <returns>True if the jump began successfully</returns>
        public bool TryBeginJump(out string failReason)
        {
            if (jumpCountController.JumpQty < JumpModel.maxJumpQty)
            {
                jump.Begin();
                failReason = string.Empty;
                return true;
            }

            failReason = MaxJumpQuantityReached;
            return false;
        }

        /// <summary>
        /// Cancels the jumping for the character
        /// </summary>
        public void CancelJump()
        {
            jump.Cancel();
        }

        /// <summary>
        /// Sets the run direction for the character
        /// </summary>
        /// <param name="value"></param>
        public void SetRunDirection(float value)
        {
            run.SetDirection(value);
        }
    }
}
