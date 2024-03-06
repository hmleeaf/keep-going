using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject moveTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        moveTarget = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (moveTarget != null)
        {
            Debug.Log("moving");
            agent.SetDestination(moveTarget.transform.position);
        }
    }
}
