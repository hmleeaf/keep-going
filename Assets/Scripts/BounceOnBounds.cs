using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BounceOnXBounds : MonoBehaviour
{
    [SerializeField] float boundsMin = -10f;
    [SerializeField] float boundsMax = 10f;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < boundsMin)
        {
            Vector3 position = transform.position;
            position.x = boundsMin;
            transform.position = position;

            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);
        }
        else if (transform.position.x > boundsMax)
        {
            Vector3 position = transform.position;
            position.x = boundsMax;
            transform.position = position;

            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);
        }
    }
}
