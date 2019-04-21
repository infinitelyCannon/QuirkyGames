using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour {

    private EventSystem eventSystem;
    private bool isPaused = false;
    public UIState currentState = null;
    private List<Toggle> statePanels;
    private List<GameObject> pages;

	// Use this for initialization
	void Start () {
        statePanels = new List<Toggle>();
        Transform tabs = transform.GetChild(0);
        pages = new List<GameObject>();

        for (int i = 0; i < tabs.childCount; i++)
            statePanels.Add(tabs.GetChild(i).GetComponent<Toggle>());

        for (int j = 0; j < transform.childCount; j++)
            if (transform.GetChild(j).CompareTag("UIState"))
                pages.Add(transform.GetChild(j).gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Start"))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GoToState(0);
            }
            else
            {
                Resume();
            }
        }

        if (isPaused && currentState != null)
            currentState.UpdateState();
	}

    public void GoToPage(int page)
    {

    }

    public void GoToState(int state)
    {
        statePanels[state].isOn = true;
        if (currentState != null)
            currentState.ExitState();
        if(pages[state].GetComponent<UIState>() != null)
            pages[state].GetComponent<UIState>().EnterState(this);
        currentState = pages[state].GetComponent<UIState>();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //GoToState(0);
        statePanels[0].isOn = false;
    }

    public void Launch()
    {
        GoToState(1);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
