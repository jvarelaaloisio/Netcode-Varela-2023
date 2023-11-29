using UnityEngine;

namespace Characters.Visuals
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private MovementState _lastState;

        private void Reset()
        {
            spriteRenderer ??= GetComponent<SpriteRenderer>();
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