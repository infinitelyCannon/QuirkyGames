using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ViewScoreState : UIState {

    public Text nameList;
    public Text pointList;

    private PauseMenuScript menuScript;
    private ScrollRect scrollRect;

    public override void UpdateState()
    {
        if (gameObject.activeSelf && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(transform.GetChild(2).gameObject);

        if (Input.GetAxisRaw("JoyStickCameraY") != 0f && scrollRect.verticalScrollbar.gameObject.activeSelf)
        {
            float delta = scrollRect.verticalNormalizedPosition;
            delta += 0.01f * scrollRect.scrollSensitivity * Input.GetAxisRaw("JoyStickCameraY");
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(delta, 0f, 1f);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f && scrollRect.verticalScrollbar.gameObject.activeSelf)
        {
            float delta = scrollRect.verticalNormalizedPosition;
            delta += scrollRect.scrollSensitivity * Input.GetAxisRaw("Mouse ScrollWheel");
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(delta, 0f, 1f);
        }
    }

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        ScoreScript.instance.Save();
        menuScript = pauseMenu;
        nameList.text = ScoreScript.instance.PrintNames();
        pointList.text = ScoreScript.instance.PrintPoints();
        scrollRect = GetComponentInChildren<ScrollRect>();
        eventSystem.SetSelectedGameObject(transform.GetChild(2).gameObject);
    }

    public override void ExitState()
    {
        
    }

    public void ScrollTrigger(BaseEventData data)
    {
        /*PointerEventData scrollData = (PointerEventData)data;
        float delta = scrollRect.verticalNormalizedPosition;

        if (scrollData.scrollDelta.y > 0f && scrollRect.verticalScrollbar.gameObject.activeSelf)
        {
            delta += 0.01f * scrollRect.scrollSensitivity;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(delta, 0f, 1f);
        }
        else if (scrollData.scrollDelta.y < 0f && scrollRect.verticalScrollbar.gameObject.activeSelf)
        {
            delta -= 0.01f * scrollRect.scrollSensitivity;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(delta, 0f, 1f);
        }*/
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
