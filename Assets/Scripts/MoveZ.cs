using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    [SerializeField] float zVelocity = 5f;

    private void Update()
    {
        transform.position = transform.position + transform.forward * zVelocity * Time.deltaTime;
    }
}
