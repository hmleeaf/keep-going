using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInOutText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
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

    float fadeInStartTime = float.MinValue;
    float fadeOutStartTime = float.MinValue;
    
    public bool FadeIn(string text)
    {
        if (state != FadeState.Hidden) return false;

        this.text.text = text;
        this.text.alpha = 0f;
        fadeInStartTime = Time.time;
        state = FadeState.FadeIn;
        gameObject.SetActive(true);
        return true;
    }

    public bool FadeIn(string text, float fadeInDuration)
    {
        bool success = FadeIn(text);
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
                    text.alpha = (Time.time - fadeInStartTime) / fadeInDuration;
                }
                else
                {
                    state = FadeState.Visible;
                }
            }

            if (state == FadeState.Visible)
            {
                text.alpha = 1f;
            }

            if (state == FadeState.FadeOut)
            {
                if (Time.time < fadeOutStartTime + fadeOutDuration)
                {
                    text.alpha = 1f - (Time.time - fadeOutStartTime) / fadeOutDuration;
                }
                else
                {
                    state = FadeState.Hidden;
                }
            }

            if (state == FadeState.Hidden)
            {
                text.alpha = 0f;
                gameObject.SetActive(false);
            }
        }
    }
}
