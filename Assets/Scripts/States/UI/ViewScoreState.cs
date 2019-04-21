using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewScoreState : UIState {

    public Text scoreTable;

    private PauseMenuScript menuScript;

    public override void UpdateState()
    {
        if (gameObject.activeSelf && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(transform.GetChild(2).gameObject);
    }

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        ScoreScript.instance.Save();
        menuScript = pauseMenu;
        scoreTable.text = ScoreScript.instance.PrintScores();
        eventSystem.SetSelectedGameObject(transform.GetChild(2).gameObject);
    }

    public override void ExitState()
    {
        
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
