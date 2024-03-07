using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmbientText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float holdTime = 1f;
    [SerializeField] float fadeOutTime = 1f;

    CinemachineBrain cinemachineBrain;
    float lifetime;

    private void Start()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    private void OnEnable()
    {
        lifetime = 0f;
        text.alpha = 0f;
    }

    private void Update()
    {
        transform.forward = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.forward;
        lifetime += Time.deltaTime;
        if (lifetime < fadeInTime)
        {
            text.alpha = Mathf.Lerp(0f, 1f, lifetime / fadeInTime);
        }
        else if (lifetime < fadeInTime + holdTime)
        {
            text.alpha = 1f;
        }
        else if (lifetime < fadeInTime + holdTime + fadeOutTime)
        {
            text.alpha = Mathf.Lerp(1f, 0f, (lifetime - fadeInTime - holdTime) / fadeOutTime);
        }
        else
        {
            text.alpha = 0;
        }
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
