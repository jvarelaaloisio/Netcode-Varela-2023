using Core.Characters;
using Core.Extensions;
using UnityEngine;

namespace Characters.Visuals
{
    public class CharacterView : MonoBehaviour, ICharacterSetup
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private MovementState _lastState;
        [SerializeField] private CharacterAnimatorController animatorController;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animatorController = GetComponent<CharacterAnimatorController>();
        }

        private void Update()
        {
            if(transform.hasChanged)
            {
                if (_lastState.position.x > transform.position.x)
                    spriteRenderer.flipX = true;
                else if (_lastState.position.x < transform.position.x)
                    spriteRenderer.flipX = false;
            }
            _lastState.Save(this);
        }

        /// <summary>
        /// Changes the base sprite on the sprite renderer
        /// </summary>
        /// <param name="newSprite"></param>
        public void OverrideBaseSprite(Sprite newSprite)
        {
            if (!spriteRenderer)
            {
                this.LogError($"{nameof(spriteRenderer)} is null!");
                return;
            }
            spriteRenderer.sprite = newSprite;
        }
        
        /// <summary>
        /// Changes the current Animator controller
        /// </summary>
        /// <param name="newController"></param>
        public void OverrideAnimatorController(RuntimeAnimatorController newController)
        {
            if (animatorController == null)
            {
                this.LogError($"{nameof(animatorController)} is null!");
                return;
            }

            animatorController.OverrideRuntimeControllerServerRPC(newController.name);
        }
        
        private struct MovementState
        {
            public Vector2 position;

            public void Save(Component component)
            {
                position = component.transform.position;
            }
        }
    }
}