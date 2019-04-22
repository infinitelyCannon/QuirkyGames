using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserPulse : MonoBehaviour {

    public float runTime;
    public float waitTime;

    private ParticleSystem particle;
    private float runTimer = 0f;
    private float waitTimer = 0f;
    private bool isSpraying = true;

	// Use this for initialization
	void Start () {
        particle = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isSpraying)
        {
            runTimer += Time.deltaTime;
            if(runTimer >= runTime)
            {
                runTimer = 0f;
                isSpraying = false;
                particle.Stop();
            }
        }
        else
        {
            waitTimer += Time.deltaTime;
            if(waitTimer >= waitTime)
            {
                waitTimer = 0f;
                isSpraying = true;
                particle.Play();
            }
        }
	}
}
