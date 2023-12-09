using System;
using System.Linq;
using Characters.Movement;
using Core.Extensions;
using UnityEngine;

public class JumpCountController : MonoBehaviour
{
    [SerializeField] private bool shouldResetOnLand = true;
    [SerializeField] private float maxFloorAngle = 15f;
    [SerializeField] private Jump jump;
    public int JumpQty { get; private set; } = 0;

    private void OnValidate()
    {
        jump ??= GetComponent<Jump>();
    }

    private void OnEnable()
    {
        if (!jump)
            this.LogError("No jump component was provided!");
        else
        {
            jump.OnBegan += HandleBeganJump;
        }
    }

    private void OnDisable()
    {
        jump.OnBegan -= HandleBeganJump;
        jump.OnCanceled -= HandleCanceledJump;
    }

    private void HandleBeganJump()
    {
        JumpQty++;
    }

    private void HandleCanceledJump()
    {
        JumpQty--;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var contacts = col.contacts;
        foreach (var contact in contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.black, 1f);
        }

        if (shouldResetOnLand
            && contacts.Any(point => Vector2.Angle(point.normal, transform.up) <= maxFloorAngle))
        {
            JumpQty = 0;
            Debug.Log($"{name} ({GetType().Name}): Jump count was reset!");
        }
    }
}