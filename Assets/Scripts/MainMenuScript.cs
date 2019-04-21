using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    private enum MainMenuState
    {
        Main,
        HowTo,
        Score,
        Credits
    }

    private EventSystem eventSystem;
    private MainMenuState menuState = MainMenuState.Main;
    private List<Toggle> statePanels;
    private List<GameObject> pages;

    public Text scoreTable;

	// Use this for initialization
	void Start () {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        statePanels = new List<Toggle>();
        Transform tabs = transform.GetChild(0);
        pages = new List<GameObject>();

        for (int j = 0; j < transform.childCount; j++)
            if (transform.GetChild(j).CompareTag("UIState"))
                pages.Add(transform.GetChild(j).gameObject);

        for (int i = 0; i < tabs.childCount; i++)
            statePanels.Add(tabs.GetChild(i).GetComponent<Toggle>());
	}
	
	// Update is called once per frame
	void Update () {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
	}

    public void MoveTrigger(BaseEventData data)
    {
        if (eventSystem.currentSelectedGameObject == gameObject)
        {
            for (int i = 0; i < pages.Count; i++)
                if (pages[i].activeSelf)
                    eventSystem.SetSelectedGameObject(pages[i].transform.GetChild(1).gameObject);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToPage(int page)
    {
        statePanels[page].isOn = true;
        eventSystem.SetSelectedGameObject(pages[page].transform.GetChild(1).gameObject);
    }

    public void HighScores()
    {
        string scores = ScoreScript.instance.PrintScores();

        if (scores == "")
            scoreTable.text = "No Submitted Scores";
        else
            scoreTable.text = scores;
        statePanels[2].isOn = true;
        eventSystem.SetSelectedGameObject(pages[2].transform.GetChild(1).gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
