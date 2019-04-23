using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - 20, 250, 20), Input.GetAxisRaw("Mouse ScrollWheel").ToString());
    }

    // Update is called once per frame
    void Update () {
        
    }
}
