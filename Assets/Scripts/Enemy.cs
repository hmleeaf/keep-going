using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hitpoints = 3;

    Wave wave;

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    public void Damage()
    {
        hitpoints--;
        if (hitpoints <= 0)
        {
            wave.RemoveEnemy(this);
            if (wave.EnemyCount <= 0)
            {
                wave.SetBarriersActive(false);
            }
            Destroy(gameObject);
        }
    }
}
