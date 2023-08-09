using Characters.Movement;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    private const string SpeedXParameter = "VelX";
    private const string JumpParameter = "Jump";

    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Jump jump;


    private MovementState _lastState;
    private readonly int speedXParameterHash = Animator.StringToHash(SpeedXParameter);
    private readonly int JumpTriggerHash = Animator.StringToHash(JumpParameter);

    private void OnValidate()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        animator ??= GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (jump) jump.OnBegan += HandleJumpBegan;
    }

    private void OnDisable()
    {
        if (jump) jump.OnBegan -= HandleJumpBegan;
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
        animator.SetFloat(speedXParameterHash, rigidbody2D.velocity.x);
        _lastState.Save(this);
    }

    private void HandleJumpBegan()
    {
        animator.SetTrigger(JumpTriggerHash);
    }

    private struct MovementState
    {
        public Vector2 position;

        public void Save(CharacterAnimatorController characterAnimatorController)
        {
            position = characterAnimatorController.transform.position;
        }
    }
}
