using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    private List<Toggle> toggles;

	// Use this for initialization
	void Start () {
        toggles = new List<Toggle>();
        for(int i = 0; i < transform.childCount; i++)
        {
            toggles.Add(transform.GetChild(i).GetComponent<Toggle>());
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            toggles[0].isOn = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            toggles[1].isOn = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggles[2].isOn = true;
        }
    }
}
