using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] EnemyAI enemy;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == null) return;
        if (collision.collider.CompareTag("Bullet"))
        {
            enemy.Aggro();
            Destroy(collision.collider.gameObject);
        }
    }
}
