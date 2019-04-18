using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOffset : MonoBehaviour {
    public Renderer rend;

    public Material material1;
    public Material material2;
    public float duration = 2f;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
	}

    // Update is called once per frame
    void FixedUpdate () {
        //Changes the to the location of the offset material and back
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        rend.material.Lerp(material1, material2, lerp);
        //Changes alpha of plane so other plane's albedo and normal is visible
        float alpha = Mathf.PingPong(Time.time, duration) / duration;
        if (rend.CompareTag("Calm") == true) {
            rend.material.color = new Color(material1.color.r, material1.color.g, material1.color.b, 1 - alpha);
        }
        else {
            rend.material.color = new Color(material1.color.r, material1.color.g, material1.color.b, alpha);
        }
    }
}
