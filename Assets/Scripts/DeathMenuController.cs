using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DeathMenuController : MonoBehaviour {

    public Text finalScore;

    private EventSystem eventSystem;

	// Use this for initialization
	void Start () {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.activeSelf && eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
	}

    public void Launch()
    {
        gameObject.SetActive(true);
        finalScore.text += " " + ScoreScript.instance.score;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
}
