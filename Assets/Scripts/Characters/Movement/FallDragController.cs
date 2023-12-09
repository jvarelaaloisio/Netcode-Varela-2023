using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Extensions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallDragController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float normalDragValue = 10;

    private void OnValidate()
    {
        rigidbody ??= GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody.drag = rigidbody.velocity.y.IsNegative() ? 0 : normalDragValue;
    }
}
