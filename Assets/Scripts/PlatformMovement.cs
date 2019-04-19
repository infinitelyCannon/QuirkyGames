using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    private float hoverTime = 0f;
    public float hoverSpeed;
    public float hoveHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hoverTime += Time.deltaTime;
        transform.position += new Vector3(0f, (Mathf.Sin(hoverTime * hoverSpeed) * hoveHeight) * Time.deltaTime, 0f);
	}
}
