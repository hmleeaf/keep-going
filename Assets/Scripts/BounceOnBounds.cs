using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnBounds : MonoBehaviour
{
    public enum Axis
    {
        X, Y, Z
    }
    [SerializeField] Axis boundAxis;
    [SerializeField] float boundsMin = 10f;
    [SerializeField] float boundsMax = -10f;

    private void FixedUpdate()
    {
        if (transform.position.x < boundsMin)
        {
            Vector3 forward = transform.forward;
            Vector3 position = transform.position;
            switch (boundAxis)
            {
                case Axis.X:
                    position.x = boundsMin;
                    forward.x = -forward.x;
                    break;
                case Axis.Y:
                    position.y = boundsMin;
                    forward.y = -forward.y;
                    break;
                case Axis.Z:
                    position.z = boundsMin;
                    forward.z = -forward.z;
                    break;
            }
            transform.forward = forward;
            transform.position = position;
        }
        else if (transform.position.x > boundsMax)
        {
            Vector3 forward = transform.forward;
            Vector3 position = transform.position;
            switch (boundAxis)
            {
                case Axis.X:
                    position.x = boundsMax;
                    forward.x = -forward.x;
                    break;
                case Axis.Y:
                    position.y = boundsMax;
                    forward.y = -forward.y;
                    break;
                case Axis.Z:
                    position.z = boundsMax;
                    forward.z = -forward.z;
                    break;
            }
            transform.forward = forward;
            transform.position = position;
        }
    }
}
