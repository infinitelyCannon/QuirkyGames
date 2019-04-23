using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public float distance;
    public float speed;

    private float azimuth = 180f;
    public float colatitude = 45f;

	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update () {
        azimuth = (azimuth + Time.deltaTime * speed) % 360f;

        transform.localPosition = new Vector3(
            distance * Mathf.Sin(Mathf.Deg2Rad * azimuth) * Mathf.Sin(Mathf.Deg2Rad * colatitude),
            distance * Mathf.Cos(Mathf.Deg2Rad * colatitude),
            distance * Mathf.Cos(Mathf.Deg2Rad * azimuth) * Mathf.Sin(Mathf.Deg2Rad * colatitude)
            );
    }
}
