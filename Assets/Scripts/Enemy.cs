using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHp = 3;

    int health;
    public int Health { get { return health; } }

    private void OnEnable()
    {
        health = maxHp;
    }

    public void Damage()
    {
        health--;
    }
}
