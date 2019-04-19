using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVolume : MonoBehaviour {

    public class CameraParameters
    {
        public float maxY;
        public float minY;
        public float minX;
        public float maxX;

        public CameraParameters(float minY, float maxY, float minX, float maxX)
        {
            this.maxY = maxY;
            this.minY = minY;
            this.minX = minX;
            this.maxX = maxX;
        }

        public CameraParameters()
        {
            maxY = 0f;
            minY = 0f;
            maxX = 0f;
            minX = 0f;
        }
    }

    private CameraController controller;

    public float minimumY;
    public float maximumY;
    public float minimumX;
    public float maximumX;

    private CameraParameters swapValues;

	// Use this for initialization
	void Start () {
        controller = Camera.main.transform.parent.parent.GetComponent<CameraController>();
        swapValues = new CameraParameters();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (controller.checkForCollision && other.CompareTag("MainCamera"))
        {
            CameraParameters parameters = new CameraParameters(minimumY, maximumY, minimumX, maximumX);
            swapValues.maxY = controller.maximumY;
            swapValues.minY = controller.minimumY;
            swapValues.maxX = controller.maximumX;
            swapValues.minX = controller.minimumX;
            controller.ReConfigure(parameters);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(controller.checkForCollision && other.CompareTag("MainCamera"))
        {
            CameraParameters parameters = new CameraParameters(swapValues.minY, swapValues.maxY, swapValues.minX, swapValues.maxX);
            controller.ReConfigure(parameters);
        }
    }
}
