using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSense : MonoBehaviour {

    public Material border;
    public Material nonBorder;

    private bool outlineOn;

	// Use this for initialization
	void Start () {
        outlineOn = false;
	}
	
	// Update is called once per frame
	void Update () {
        ToggleOutline();
	}

    void ToggleOutline() {
        if (Input.GetButtonDown("Fire3") && outlineOn == false) {
            GetComponent<Renderer>().material = border;
            outlineOn = true;
        } else if(Input.GetButtonDown("Fire3") && outlineOn == true) {
            GetComponent<Renderer>().material = nonBorder;
            outlineOn = false;
        }
    }
}
