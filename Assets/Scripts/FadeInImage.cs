using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float fadeInDuration = 1f;
    [SerializeField] float fadeOutDuration = 1f;

    enum FadeState
    {
        FadeIn,
        Visible,
        FadeOut,
        Hidden,
    }
    FadeState state = FadeState.Hidden;

    Color color;

    float fadeInStartTime = float.MinValue;
    float fadeOutStartTime = float.MinValue;

    public bool FadeIn()
    {
        if (state != FadeState.Hidden) return false;

        Color color = image.color;
        color.a = 0f;
        image.color = color;
        fadeInStartTime = Time.time;
        state = FadeState.FadeIn;
        gameObject.SetActive(true);
        return true;
    }

    public bool FadeIn(float fadeInDuration)
    {
        bool success = FadeIn();
        if (success)
        {
            this.fadeInDuration = fadeInDuration;
        }
        return success;
    }

    public bool FadeOut()
    {
        if (state != FadeState.Visible) return false;

        fadeOutStartTime = Time.time;
        state = FadeState.FadeOut;
        return true;
    }

    public bool FadeOut(float fadeOutDuration)
    {
        bool success = FadeOut();
        if (success)
        {
            this.fadeOutDuration = fadeOutDuration;
        }
        return success;
    }

    private void Update()
    {

        if (gameObject.activeSelf)
        {
            if (state == FadeState.FadeIn)
            {
                if (Time.time < fadeInStartTime + fadeInDuration)
                {
                    color.a = (Time.time - fadeInStartTime) / fadeInDuration;
                }
                else
                {
                    state = FadeState.Visible;
                }
            }

            if (state == FadeState.Visible)
            {
                color.a = 1f;
            }

            if (state == FadeState.FadeOut)
            {
                if (Time.time < fadeOutStartTime + fadeOutDuration)
                {
                    color.a = 1f - (Time.time - fadeOutStartTime) / fadeOutDuration;
                }
                else
                {
                    state = FadeState.Hidden;
                }
            }

            if (state == FadeState.Hidden)
            {
                color.a = 0f;
                gameObject.SetActive(false);
            }

            image.color = color;
        }
    }
}
