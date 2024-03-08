using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] int maxHp = 100;

    int health;
    public int Health { get { return health; } }
    public int MaxHp { get { return maxHp; } }

    private void OnEnable()
    {
        health = maxHp;
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
}
