using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVolume : MonoBehaviour {

    public class CameraParameters
    {
        public float maxY;
        public float minY;

        public CameraParameters(float minY, float maxY)
        {
            this.maxY = maxY;
            this.minY = minY;
        }
    }

    private CameraController controller;

    public float minimumY;
    public float maximumY;

    private float swapMaxY;
    private float swapMinY;

	// Use this for initialization
	void Start () {
        controller = Camera.main.transform.parent.parent.GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (controller.checkForCollision && other.CompareTag("MainCamera"))
        {
            CameraParameters parameters = new CameraParameters(minimumY, maximumY);
            swapMaxY = controller.maximumY;
            swapMinY = controller.minimumY;
            controller.ReConfigure(parameters);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(controller.checkForCollision && other.CompareTag("MainCamera"))
        {
            CameraParameters parameters = new CameraParameters(swapMinY, swapMaxY);
            controller.ReConfigure(parameters);
        }
    }
}
