using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionState : UIState {

    public float fadeTime;

    private Color currentColor;
    private Image image;
    private float lerpVelocity = 0f;
    private float lerpValue = 0f;
    private bool toAlpha;

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        image = GetComponent<Image>();
        currentColor = image.color;
        if (currentColor.a == 0f)
            toAlpha = false;
        else
            toAlpha = true;
    }

    public override void UpdateState()
    {
        if (toAlpha)
        {
            lerpValue = Mathf.SmoothDamp(lerpValue, 1f, ref lerpVelocity, fadeTime);
            currentColor = Color.Lerp(Color.black, Color.clear, lerpValue);
        }
        else
        {
            lerpValue = Mathf.SmoothDamp(lerpValue, 1f, ref lerpVelocity, fadeTime);
            currentColor = Color.Lerp(Color.clear, Color.black, lerpValue);
        }
        image.color = currentColor;
    }

    public override void ExitState()
    {
        
    }
}
