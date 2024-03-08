using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SaturationPostProcess : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] float minDistanceToStartDesaturate = 200f;

    Volume volume;
    ColorAdjustments colorAdjustments;

    float initialSaturationValue;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        initialSaturationValue = colorAdjustments.saturation.value;
    }

    private void OnDisable()
    {
        colorAdjustments.saturation.value = initialSaturationValue;
    }

    private void Update()
    {
        UpdateSaturation();
    }

    void UpdateSaturation()
    {
        float progress = gameController.Progress;
        float t = (progress - minDistanceToStartDesaturate) / (gameController.TransitionableDistance - minDistanceToStartDesaturate);
        colorAdjustments.saturation.value = Mathf.Lerp(0, -100, t);
    }
}
