using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacGuff : MonoBehaviour {

    public ParticleSystem particle;

	// Use this for initialization
	void Start () {
        particle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Sense"))
            particle.Play();
        else if (Input.GetButtonUp("Sense"))
            particle.Stop();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(particle);
            Destroy(this);
        }
    }
}
