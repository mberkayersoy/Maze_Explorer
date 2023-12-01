using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius;
    [SerializeField] private bool showDebug;
    private bool isGrounded;

    public event Action<bool> OnIsGroundedChangeAction;

    private void FixedUpdate()
    {
        CheckGround();
    }
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundRadius, groundLayer);
        OnIsGroundedChangeAction?.Invoke(isGrounded);
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = isGrounded ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);

            Gizmos.DrawSphere(transform.position, groundRadius);
        }

    }

}

