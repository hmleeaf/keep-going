using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroyDestructible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDestructible destructible = other.gameObject.GetComponent<IDestructible>();
        if (destructible != null)
        {
            destructible.Destruct();
            Destroy(this.gameObject);
        }
    }
}
