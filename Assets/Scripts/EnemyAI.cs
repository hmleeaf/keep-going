using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] float maxMoveTime = 2f;
    [SerializeField] float minXMove = -9.5f;
    [SerializeField] float maxXMove = 9f;
    [SerializeField] float aggroRadius = 20f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float attackInterval = 2f;
    [SerializeField] float bulletHeightFromFeet = 1f;

    NavMeshAgent agent;
    PlayerController playerController;
    bool aggroed = false;
    Vector3 destination;
    float lastAttackTime = float.MinValue;
    float lastMoveTime = float.MinValue;
    Entity entity;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        playerController = FindObjectOfType<PlayerController>();
        entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        aggroed = false;
    }

    private void Update()
    {
        if (!aggroed && Vector3.Distance(transform.position, playerController.transform.position) < aggroRadius)
        {
            aggroed = true;
        }

        if (entity.Health < entity.MaxHp)
        {
            aggroed = true;
        }

        if (aggroed)
        {
            Attack();
            Movement();
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        Vector3 newForward = playerController.transform.position - transform.position;
        newForward.y = 0f;
        newForward.Normalize();
        transform.forward = newForward;
    }

    void Movement()
    {
        if (destination == null || Vector3.Distance(transform.position, destination) < 0.1f || Time.time > lastMoveTime + maxMoveTime)
        {
            destination = transform.position;
            destination.x = Random.Range(minXMove, maxXMove);
            agent.SetDestination(destination);

            lastMoveTime = Time.time;
        }
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + attackInterval)
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + bulletHeightFromFeet,
                transform.position.z
            );
            bulletObj.GetComponent<Rigidbody>().velocity = (playerController.transform.position - bulletObj.transform.position).normalized;

            lastAttackTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        if (destination != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(destination, 1f);
        }

        if (playerController != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(playerController.transform.position, 1f);
        }
    }

    public void ResetAggro()
    {
        aggroed = false;
    }
}
