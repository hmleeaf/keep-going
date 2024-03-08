using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionOnTriggerStay : MonoBehaviour
{
    [SerializeField] UnityEvent action;
    [SerializeField] string otherTag;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            action.Invoke();
        }
    }
}
