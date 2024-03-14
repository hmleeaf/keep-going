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
        if (gameController.State == GameController.GameState.Lorule)
        {
            float progress = gameController.Progress;
            float t = Mathf.Min(0f, progress);
            t = t / (gameController.EndDistance + 120f);
            colorAdjustments.saturation.value = Mathf.Lerp(-100, 0, t);
        } 
        else
        {
            float progress = gameController.Progress;
            float t = (progress - minDistanceToStartDesaturate) / (gameController.TransitionableDistance - minDistanceToStartDesaturate);
            colorAdjustments.saturation.value = Mathf.Lerp(0, -100, t);
        }
    }
}
