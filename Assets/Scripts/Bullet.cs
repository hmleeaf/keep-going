using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] int maxBounce = 5;
    [SerializeField] float boundsMin = -10f;
    [SerializeField] float boundsMax = 10f;
    [SerializeField] float velocityMagnitude = 40f;
    [SerializeField] int damage = 20;
    [SerializeField] AudioClip enemyHurtClip;
    [SerializeField] AudioClip playerHurtClip;
    [SerializeField] AudioClip bounceClip;

    Rigidbody rb;
    int bounceCount = 0;
    AudioSource sfxSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sfxSource = GameObject.FindGameObjectWithTag("SFX Audio Source").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        ConstantVelocity();
        BounceOnBounds();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Entity entity = collision.collider.gameObject.GetComponent<Entity>();
        if (entity)
        {
            entity.Damage(damage);

            if (entity.CompareTag("Enemy"))
            {
                sfxSource.PlayOneShot(enemyHurtClip, 1f);
            }
            else if (entity.CompareTag("Player"))
            {
                sfxSource.PlayOneShot(playerHurtClip, 1f);
            }
            Destroy(gameObject);
        }

        Bounce();
    }

    void ConstantVelocity()
    {
        if (rb.velocity.magnitude > 0.001f)
        {
            Vector3 newVel = rb.velocity;
            newVel.y = 0f;
            rb.velocity = newVel.normalized * velocityMagnitude;
        }
    }

    void BounceOnBounds()
    {
        if (transform.position.x < boundsMin)
        {
            Vector3 position = transform.position;
            position.x = boundsMin;
            transform.position = position;

            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);

            Bounce();
        }
        else if (transform.position.x > boundsMax)
        {
            Vector3 position = transform.position;
            position.x = boundsMax;
            transform.position = position;

            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);

            Bounce();
        }
    }

    void Bounce()
    {
        bounceCount++;
        if (bounceCount > maxBounce)
        {
            Destroy(gameObject);
        }
        else
        {
            sfxSource.PlayOneShot(bounceClip, 1f);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
