using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BounceOnCollide : MonoBehaviour
{
    [SerializeField] string colliderTag;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (colliderTag == null) return;
        if (!collision.collider.CompareTag(colliderTag)) return;

        ContactPoint contact = collision.GetContact(0);
        Debug.Log("Before: " + rb.velocity);
        rb.velocity = Vector3.Reflect(rb.velocity, contact.normal);
        Debug.Log("After: " + rb.velocity);
    }
}
