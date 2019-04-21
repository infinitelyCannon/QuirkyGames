using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuState : UIState {

    private PauseMenuScript menuScript;

    public override void UpdateState()
    {
        if (gameObject.activeSelf && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(transform.GetChild(1).gameObject);

        if (Input.GetButtonDown("Start"))
            Resume();
    }

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        menuScript = pauseMenu;
        Time.timeScale = 0f;
        eventSystem.SetSelectedGameObject(transform.GetChild(1).gameObject);
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    public override void ExitState()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Time.timeScale = 1f;
        menuScript = null;
        eventSystem.SetSelectedGameObject(null);
    }

    public void Resume()
    {
        menuScript.Resume();
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
