using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatformMovement : MonoBehaviour {

    private float hoverTime = 0f;
    public float hoverSpeed;
    public float hoveHeight;

    private Rigidbody body;
    private float direction = 1f;
    private Vector3 initial;

    private float min = 0f;
    private float max = 0f;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        initial = transform.position;
	}

    // Update is called once per frame
    void Update() {
        hoverTime += Time.deltaTime;

        
	}

    private void OnGUI()
    {
        if (transform.position.y < min)
            min = transform.position.y;
        if (transform.position.y > max)
            max = transform.position.y;

        GUI.Label(new Rect(Screen.width - 160, Screen.height - 20, 160, 20), "Min: " + min.ToString());
        GUI.Label(new Rect(Screen.width - 160, Screen.height - 45, 160, 20), "Max: " + max.ToString());
    }
}
