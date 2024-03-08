using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] int maxHp = 100;
    [SerializeField] bool autoRegen = false;
    [SerializeField] int autoRegenPerTick = 1;
    [SerializeField] float autoRegenTick = 0.2f;
    [SerializeField] float autoRegenWait = 1f;

    public int health;
    public int Health { get { return health; } }
    public int MaxHp { get { return maxHp; } }

    float lastDamaged = float.MinValue;

    private void OnEnable()
    {
        health = maxHp;
        if (autoRegen)
        {
            StartCoroutine(Regen());
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        lastDamaged = Time.time;
    }

    public void HealToFull()
    {
        health = maxHp;
    }

    IEnumerator Regen()
    {
        if (Time.time > lastDamaged + autoRegenWait)
        {
            health = Mathf.Min(health + autoRegenPerTick, maxHp);
        }
        yield return new WaitForSeconds(autoRegenTick);
        StartCoroutine(Regen());
    }
}
