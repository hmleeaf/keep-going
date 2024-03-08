using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessWithProgress : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] Entity playerEntity;
    [SerializeField] float minDistanceToStartDesaturate = 200f;

    Volume volume;
    Vignette vignette;
    ColorAdjustments colorAdjustments;

    float initialVignetteValue, initialSaturationValue;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        initialVignetteValue = vignette.intensity.value;
        volume.profile.TryGet(out colorAdjustments);
        initialSaturationValue = colorAdjustments.saturation.value;
    }

    private void OnDisable()
    {
        vignette.intensity.value = initialSaturationValue;
        colorAdjustments.saturation.value = initialSaturationValue;
    }

    private void Update()
    {
        UpdateVignette();
        UpdateSaturation();
    }

    void UpdateVignette()
    {
        float t = 1 - (float)playerEntity.Health / playerEntity.MaxHp;
        t = 1 - Mathf.Pow(1 - t, 3);
        vignette.intensity.value = t;
    }

    void UpdateSaturation()
    {
        float progress = gameController.Progress;
        float t = (progress - minDistanceToStartDesaturate) / (gameController.TransitionableDistance - minDistanceToStartDesaturate);
        colorAdjustments.saturation.value = Mathf.Lerp(0, -100, t);
    }
}
