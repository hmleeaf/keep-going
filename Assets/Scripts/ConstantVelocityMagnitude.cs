using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConstantVelocityMagnitude : MonoBehaviour
{
    [SerializeField] float velocityMagnitude = 40f;
    [SerializeField] bool zeroX;
    [SerializeField] bool zeroY;
    [SerializeField] bool zeroZ;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0.001f)
        {
            Vector3 newVel = rb.velocity;
            if (zeroX) newVel.x = 0f;
            if (zeroY) newVel.y = 0f;
            if (zeroZ) newVel.z = 0f;
            rb.velocity = newVel.normalized * velocityMagnitude;
        }
    }
}
