using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraController{
    public float ySpeed;
    public float xSpeed;

    [Tooltip("The max distance the camera will be, if nothing is blocking its view.")]
    public float distance;
    public bool invertedX = true;
    public bool invertedY = false;
    public bool lockCursor = true;
    public float MinimumY;
    public float MaximumY;
    public bool checkForCollision;
    public float collisionCheckSize;

    //private Vector3 mCharacterPosition;
    //private Vector3 mCameraPosition;
    //private Vector2 mUnitPosition;
    //private float yOffset;
    private float yPos;
    private Transform mCamera;
    private bool mCursorLocked = true;
    private PlayerController mPlayer;
    private float azimuth = 180f;
    private float colatitude = 90f;
    private float pastAzimuth = 180f;

    public void Init(PlayerController player, Transform camera)
    {
        Vector3 unit3D = camera.position.normalized;
        mPlayer = player;
        //mCameraPosition = camera.position;
        //mCharacterPosition = character.position;
        //mUnitPosition = new Vector2(unit3D.x, unit3D.z);
        //distance = 10f;//camera.position.magnitude;
        //yOffset = camera.GetChild(0).position.y;
        //yPos = camera.GetChild(0).localPosition.y;
        mCamera = camera;
    }

    public void UpdatePosition()
    {
        float horizontal = Input.GetAxisRaw("Mouse X") * xSpeed * Time.deltaTime;
        float vertical = Input.GetAxisRaw("Mouse Y") * ySpeed * Time.deltaTime;

        horizontal *= invertedX ? -1f : 1f;
        vertical *= invertedY ? -1f : 1f;

        azimuth = (azimuth + horizontal) % 360f;
        colatitude += vertical;
        if (MinimumY < -180f || MaximumY > 180f)
            colatitude = Mathf.Clamp(colatitude, -180f, 180f);
        else
            colatitude = Mathf.Clamp(colatitude, MinimumY, MaximumY);

        mPlayer.AddSpinInput(azimuth - pastAzimuth);
        pastAzimuth = azimuth;

        mCamera.localPosition = new Vector3(
            distance * Mathf.Sin(Mathf.Deg2Rad * azimuth) * Mathf.Sin(Mathf.Deg2Rad * colatitude),
            distance * Mathf.Cos(Mathf.Deg2Rad * colatitude),
            distance * Mathf.Cos(Mathf.Deg2Rad * azimuth) * Mathf.Sin(Mathf.Deg2Rad * colatitude)
            );
        /*
        
        yPos += vertical * Time.deltaTime;
        yPos = Mathf.Clamp(yPos, MinimumY, MaximumY);

        tilt.y = yPos;

        mCamera.Rotate(Vector3.up, horizontal * Time.deltaTime);
        mCamera.GetChild(0).localPosition = tilt;
        */
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.L))
            mCursorLocked = false;
        else if (Input.GetMouseButtonUp(0))
            mCursorLocked = true;

        if (mCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
