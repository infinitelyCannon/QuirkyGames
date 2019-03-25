using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{

    public float horizontalAngle = 270f;
    public float verticalAngle = 0f;
    public float MinimumY = -90f;
    public float MaximumY = 90f;

    //public float azimuth = 270f;
    //public float colatitude = 180f;
    public float radius = 5f;

    /*
        Notes:
        Using a 2D unit circle, map the sine value to the colatitude like so:
        input: [1, -1]
        output: [0, 180]
    */

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float clampedAzimuth;// = azimuth % 360f;
        float clampedColatitude;// = Mathf.Clamp(colatitude, -180f, 180f);
        float testA, testC;

        if (Input.GetKeyDown(KeyCode.M))
        {
            MapToSphere(horizontalAngle, verticalAngle, out testA, out testC);
            Debug.Log("Azimuth: " + testA + " Colatitude: " + testC);
        }

        MapToSphere(horizontalAngle, verticalAngle, out clampedAzimuth, out clampedColatitude);

        transform.localPosition = new Vector3(
            radius * Mathf.Sin(Mathf.Deg2Rad * clampedAzimuth) * Mathf.Sin(Mathf.Deg2Rad * clampedColatitude),
            radius * Mathf.Cos(Mathf.Deg2Rad * clampedColatitude),
            radius * Mathf.Cos(Mathf.Deg2Rad * clampedAzimuth) * Mathf.Sin(Mathf.Deg2Rad * clampedColatitude)
            );
    }

    private void MapToSphere(float horiz, float vert, out float azimuth, out float colatitude)
    {
        azimuth = horiz % 360f;
        colatitude = Mathf.Clamp(vert, MinimumY, MaximumY);

        if (MinimumY < -180f || MaximumY > 180f)
            colatitude = Mathf.Clamp(vert, -180f, 180f);
    }
}
