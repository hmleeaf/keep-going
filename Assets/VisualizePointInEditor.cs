using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizePointInEditor : MonoBehaviour
{
    [SerializeField] Color pointColor = Color.blue;

    private void OnDrawGizmos()
    {
        Gizmos.color = pointColor;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
