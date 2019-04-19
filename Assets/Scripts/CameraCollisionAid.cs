using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionAid : MonoBehaviour {

    private SphereCollider sphereCollider;
    private CameraController controller;

    // Use this for initialization
    void Start()
    {
        controller = transform.parent.parent.GetComponent<CameraController>();
        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.radius = controller.sphereCastRadius;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (controller.checkForCollision)
        {

        }
    }
}
