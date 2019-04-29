using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour {

    private PlayerController player;
    private bool ready = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !ready){
            ready = true;
            Next(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Next(int s)
    {
        player.deathScreen.FadeIn(s);
    }
}
