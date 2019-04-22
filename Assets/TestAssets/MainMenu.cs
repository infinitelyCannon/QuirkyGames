using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    private ParticleSystem particle;

	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (particle.isPlaying)
                particle.Pause();
            else
                particle.Play();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            particle.Stop();
        }
    }
}
