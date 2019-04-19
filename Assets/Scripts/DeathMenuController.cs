using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenuController : MonoBehaviour {

    public Text finalScore;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Launch()
    {
        gameObject.SetActive(true);
        finalScore.text += " " + ScoreScript.instance.score;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
