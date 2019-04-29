using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionState : UIState {

    public float fadeTime;

    private Color currentColor;
    private Image image;
    private float lerpVelocity = 0f;
    private float lerpValue = 0f;
    private bool toAlpha;
    private PauseMenuScript menuScript;

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        image = GetComponent<Image>();
        currentColor = image.color;

        toAlpha = false;

        menuScript = pauseMenu;
    }

    public override void UpdateState()
    {
        if (toAlpha)
        {
            lerpValue += fadeTime * Time.deltaTime; //Mathf.SmoothDamp(lerpValue, 1f, ref lerpVelocity, fadeTime);
            lerpValue = Mathf.Clamp(lerpValue, 0f, 1f);
            currentColor = Color.Lerp(Color.black, Color.clear, lerpValue);
        }
        else
        {
            lerpValue += fadeTime * Time.deltaTime; //Mathf.SmoothDamp(lerpValue, 1f, ref lerpVelocity, fadeTime);
            lerpValue = Mathf.Clamp(lerpValue, 0f, 1f);
            currentColor = Color.Lerp(Color.clear, Color.black, lerpValue);
        }
        image.color = currentColor;

        if (lerpValue == 1.0f)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(menuScript.nextScene);
        }
        
    }

    public override void ExitState()
    {
        
    }
}
