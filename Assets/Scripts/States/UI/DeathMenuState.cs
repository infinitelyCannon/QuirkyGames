using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenuState : UIState {

    public Text finalScore;

    private PauseMenuScript menuScript;

    public override void UpdateState()
    {
        if (gameObject.activeSelf && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(transform.GetChild(1).gameObject);
    }

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        Time.timeScale = 0f;
        menuScript = pauseMenu;
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        finalScore.text = "Your Score: " + ScoreScript.instance.score;
        eventSystem.SetSelectedGameObject(transform.GetChild(1).gameObject);
    }

    public override void ExitState()
    {
        menuScript = null;
        eventSystem.SetSelectedGameObject(null);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        ScoreScript.instance.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnterScore()
    {
        menuScript.GoToState(2);
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
