using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private float progress = 0;
    Vector3 initialCameraPosition;

    private void Start()
    {
        initialCameraPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        progress = Mathf.Max(progress, playerController.transform.position.z);
        Vector3 newCamPos = initialCameraPosition + Vector3.forward * progress;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCamPos, 1 / Vector3.Distance(Camera.main.transform.position, newCamPos));
    }
}
