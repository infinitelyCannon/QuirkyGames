using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraController{
    public float ySpeed;
    public float xSpeed;

    public enum CameraState
    {
        Third,
        Transition,
        First
    };

    [Tooltip("The max distance the camera will be, if nothing is blocking its view.")]
    public float distance;
    [Tooltip("The smallest distance the camera will move to when something is blocking it.")]
    public float minDistance;
    public bool invertedX = true;
    public bool invertedY = false;
    public bool lockCursor = true;
    public float MinimumY;
    public float MaximumY;
    public bool checkForCollision;
    public float aimDelay;
    //public float collisionCheckSize;

    private float yPos;
    private Transform mCamera;
    private bool mCursorLocked = true;
    private PlayerController mPlayer;
    private float azimuth = 180f;
    private float colatitude = 90f;
    private CameraState cameraState = CameraState.Third;
    private float transitionTime = 0f;
    private Vector3 stateTarget;

    public void Init(PlayerController player, Transform camera)
    {
        Vector3 unit3D = camera.position.normalized;
        mPlayer = player;
        mCamera = camera;
    }

    public void UpdatePosition()
    {
        switch (cameraState)
        {
            case CameraState.Third:
                float horizontal = Input.GetAxisRaw("Camera X") * xSpeed * Time.deltaTime;
                float vertical = Input.GetAxisRaw("Camera Y") * ySpeed * Time.deltaTime;

                horizontal *= invertedX ? -1f : 1f;
                vertical *= invertedY ? -1f : 1f;

                azimuth = (azimuth + horizontal) % 360f;
                colatitude += vertical;
                if (MinimumY < -180f || MaximumY > 180f)
                    colatitude = Mathf.Clamp(colatitude, -180f, 180f);
                else
                    colatitude = Mathf.Clamp(colatitude, MinimumY, MaximumY);

                mCamera.localPosition = new Vector3(
                    distance * Mathf.Sin(Mathf.Deg2Rad * azimuth) * Mathf.Sin(Mathf.Deg2Rad * colatitude),
                    distance * Mathf.Cos(Mathf.Deg2Rad * colatitude),
                    distance * Mathf.Cos(Mathf.Deg2Rad * azimuth) * Mathf.Sin(Mathf.Deg2Rad * colatitude)
                    );
                if(Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f)
                {
                    transitionTime += Time.deltaTime;
                    if(transitionTime >= aimDelay)
                    {
                        cameraState = CameraState.Transition;
                        transitionTime = 0f;
                        stateTarget = new Vector3(0, 0, 1f);
                        //mCamera.GetComponent<Look>
                    }
                }
                break;
            default:
                break;
        }
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
