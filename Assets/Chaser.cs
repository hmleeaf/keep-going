using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chaser : MonoBehaviour
{
    [SerializeField] int damagePerTick = 1;
    [SerializeField] float timePerTick = 0.2f;

    Entity playerEntity;
    
    public bool PlayerInTrigger => playerEntity != null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEntity = other.gameObject.GetComponent<Entity>();
            StartCoroutine(Damage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEntity = null;
        }
    }

    IEnumerator Damage()
    {
        if (playerEntity != null)
        {
            playerEntity.Damage(damagePerTick);
            yield return new WaitForSeconds(timePerTick);
            StartCoroutine(Damage());
        }
    }
}
