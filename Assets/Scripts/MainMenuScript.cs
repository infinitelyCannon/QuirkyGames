using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    private EventSystem eventSystem;
    private List<Toggle> statePanels;
    private List<GameObject> pages;

    public Text nameList;
    public Text scoreList;
    public ScrollRect scrollRect;

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

        if (Input.GetAxisRaw("JoyStickCameraY") != 0f && scrollRect.verticalScrollbar.gameObject.activeSelf)
        {
            float delta = scrollRect.verticalNormalizedPosition;
            delta += 0.01f * scrollRect.scrollSensitivity * Input.GetAxisRaw("JoyStickCameraY");
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(delta, 0f, 1f);
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
        string names = ScoreScript.instance.PrintNames();
        string scores = ScoreScript.instance.PrintPoints();

        if (names == "" || scores == "")
            nameList.text = "No Submitted Scores";
        else
        {
            nameList.text = names;
            scoreList.text = scores;
        }
        statePanels[2].isOn = true;
        eventSystem.SetSelectedGameObject(pages[2].transform.GetChild(1).gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
