using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionOnTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent action;
    [SerializeField] string otherTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            action.Invoke();
        }
    }
}
