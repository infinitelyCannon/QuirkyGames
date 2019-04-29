using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeControl : MonoBehaviour {

    public bool invertX;
    public bool invertY;
    public float turnSpeed;
    public float moveSpeed;
    public float maxY;
    public float minY;

    float lookAngle = 0f;
    float tiltAngle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float cameraX = Input.GetAxisRaw("Camera X");
        float cameraY = Input.GetAxisRaw("Camera Y");
        float upDown = 0f;

        cameraX *= invertX ? -1f : 1f;
        cameraY *= invertX ? -1f : 1f;

        lookAngle += cameraX * turnSpeed;
        tiltAngle -= cameraY * turnSpeed;

        tiltAngle = Mathf.Clamp(tiltAngle, -minY, maxY);

        Quaternion targetRotation = Quaternion.Euler(tiltAngle, lookAngle, 0f);

        transform.localRotation = targetRotation;

        if (Input.GetButton("Jump"))
            upDown = 1f;
        else if (Input.GetButton("Sense"))
            upDown = -1f;

        /*Vector3 targetMove = new Vector3(
            horizontal * moveSpeed * Time.deltaTime,
            upDown * moveSpeed * Time.deltaTime,
            vertical* moveSpeed * Time.deltaTime);*/

        transform.position += (transform.forward * moveSpeed * vertical * Time.deltaTime) + 
            (transform.right * horizontal * moveSpeed * Time.deltaTime) + 
            (transform.up * upDown * moveSpeed * Time.deltaTime);
    }
}
