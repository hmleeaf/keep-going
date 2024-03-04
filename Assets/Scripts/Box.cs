using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDestructible
{
    public void Destruct()
    {
        Destroy(gameObject);
    }
}
