using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class VerticalMenuScript : MonoBehaviour {

    public GameObject howTo;
    public GameObject playBtn;
    public GameObject howBtn;
    public GameObject quitBtn;
    public GameObject backBtn;

    private EventSystem eventSystem;

	// Use this for initialization
	void Start () {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowTo()
    {
        howTo.SetActive(true);
        playBtn.SetActive(false);
        howBtn.SetActive(false);
        quitBtn.SetActive(false);
        eventSystem.SetSelectedGameObject(backBtn);
    }

    public void Back()
    {
        howTo.SetActive(false);
        playBtn.SetActive(true);
        howBtn.SetActive(true);
        quitBtn.SetActive(true);
        eventSystem.SetSelectedGameObject(playBtn);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
