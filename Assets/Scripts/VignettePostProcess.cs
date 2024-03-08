using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class VignettePostProcess : MonoBehaviour
{
    [SerializeField] Entity playerEntity;

    Volume volume;
    Vignette vignette;

    float initialVignetteValue;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        initialVignetteValue = vignette.intensity.value;
    }

    private void OnDisable()
    {
        vignette.intensity.value = initialVignetteValue;
    }

    private void Update()
    {
        UpdateVignette();
    }

    void UpdateVignette()
    {
        float t = 1 - (float)playerEntity.Health / playerEntity.MaxHp;
        t = 1 - Mathf.Pow(1 - t, 3);
        vignette.intensity.value = t;
    }
}
