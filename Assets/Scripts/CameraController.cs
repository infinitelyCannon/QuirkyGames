using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class CameraTarget
{
    public Transform target;
    public Vector2 offset;
    public float distance;

    public CameraTarget()
    {
        target = null;
        offset = Vector2.zero;
        distance = 0f;
    }

    public CameraTarget(Transform target, Vector2 offset, float distance)
    {
        this.target = target;
        this.offset = offset;
        this.distance = distance;
    }
}

public class RayHitComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
    }
}

public class CameraController : MonoBehaviour{
    public enum UpdateType
    {
        FixedUpdate,
        LateUpdate,
        ManualUpdate
    }

    [SerializeField] private GameObject aimReference;
    [Header("Camera Movement, Targets, and Rotaion")]
    [SerializeField] private float moveSpeed;
    [Range(0f, 10f)] [SerializeField] private float turnSpeed;
    [SerializeField] private float turnSmoothing = 1f;
    public float maximumY = 75f;
    public float minimumY = 45f;
    public float minimumX = Mathf.NegativeInfinity;
    public float maximumX = Mathf.Infinity;
    [SerializeField] private bool lockCursor;
    [Tooltip("The list of stored targets. The camera will follow 1st target by default, others can be cycled through.")]
    [SerializeField] private CameraTarget[] targets;
    [SerializeField] private UpdateType updateType;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    //List of variables responsible for wall detection
    [Header("Physics variables to help avoid clipping")]
    public bool checkForCollision;
    [Tooltip("Time it takes to move when avoiding objects. Lower numbers are faster")]
    public float moveTime = 0.05f;
    public float returnTime = 0.4f;
    public float sphereCastRadius = 0.1f;
    public float closestDistance = 0.5f;
    public bool dodging { get; private set; }
    public LayerMask physicsLayersToCheckFor;

    private float initialDistance;
    private float cameraVelocity;
    private float currentDistance;
    private Ray checkRay = new Ray();
    private RaycastHit[] rayHits;
    private RayHitComparer hitComparer;
    

    //Private variables for camera movement and rotation
    private CameraTarget currentTarget = null;
    private int targetIndex;
    private Transform cameraTransform;
    private Transform pivotTransform;
    private Vector3 lastTargetPosition;
    private float lookAngle;
    private float tiltAngle;
    private Vector3 pivotEulers;
    private Quaternion pivotTargetRotation;
    private Quaternion transformTargetRotation;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        pivotTransform = cameraTransform.parent;

        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;

        pivotEulers = pivotTransform.rotation.eulerAngles;
        pivotTargetRotation = pivotTransform.localRotation;

        transformTargetRotation = transform.localRotation;
    }

    private void Start()
    {
        if (targets.Length == 0)
            FindAndTargetPlayer();

        if(targets.Length > 0)
        {
            currentTarget = targets[0];
            targetIndex = 0;

            initialDistance = currentTarget.distance;
            currentDistance = initialDistance;
            pivotTransform.localPosition = new Vector3(currentTarget.offset.x, currentTarget.offset.y, 0f);

            hitComparer = new RayHitComparer();

            sphereCollider = GetComponent<SphereCollider>();
        }
        else
        {
            Debug.LogWarning("No active GameObject found with tag \"Player\", script will not continue.");
        }
    }

    public void FindAndTargetPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            targets = new CameraTarget[] { new CameraTarget(player.transform, Vector2.zero, -5) };
        }
    }

    public void ReConfigure(CameraVolume.CameraParameters parameters)
    {
        minimumY = parameters.minY;
        maximumY = parameters.maxY;
        minimumX = parameters.minX;
        maximumX = parameters.maxX;
    }

    private void Update()
    {
        HandleRotation();
        if(lockCursor && Input.GetMouseButtonUp(0) && Time.timeScale > 0f)
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }

        if (Input.GetButtonDown("Aim") || Input.GetAxisRaw("Aim") == 1f)
        {
            SetTarget(1);
            aimReference.SetActive(true);
        }

        if (Input.GetButtonUp("Aim") || Input.GetAxisRaw("Aim") == 0f)
        {
            SetTarget(0);
            aimReference.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (currentTarget == null && (targets.Length == 0 || AllTargetsDisabled()))
            return;
        if (updateType == UpdateType.FixedUpdate)
            FollowTarget(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (currentTarget == null && (targets.Length == 0 || AllTargetsDisabled()))
            return;
        if (updateType == UpdateType.LateUpdate)
            FollowTarget(Time.deltaTime);

        if (checkForCollision)
        {
            float targetDistance = initialDistance;

            checkRay.origin = pivotTransform.position + pivotTransform.forward * sphereCastRadius;
            checkRay.direction = -pivotTransform.forward;

            Collider[] colliders = Physics.OverlapSphere(checkRay.origin, sphereCastRadius, physicsLayersToCheckFor);

            bool initialIntersect = false;
            bool hitSomething = false;

            for(int i = 0; i < colliders.Length; i++)
            {
                if((!colliders[i].isTrigger) && colliders[i].attachedRigidbody == null)
                {
                    initialIntersect = true;
                    break;
                }
            }

            if (initialIntersect)
            {
                checkRay.origin += pivotTransform.forward * sphereCastRadius;

                rayHits = Physics.RaycastAll(checkRay, initialDistance - sphereCastRadius, physicsLayersToCheckFor);
            }
            else
            {
                rayHits = Physics.SphereCastAll(checkRay, sphereCastRadius, initialDistance + sphereCastRadius, physicsLayersToCheckFor);
            }

            System.Array.Sort(rayHits, hitComparer);

            float nearest = Mathf.Infinity;

            for(int i = 0; i < rayHits.Length; i++)
            {
                if(rayHits[i].distance < nearest && (!rayHits[i].collider.isTrigger) && rayHits[i].collider.attachedRigidbody == null)
                {
                    nearest = rayHits[i].distance;
                    targetDistance = -pivotTransform.InverseTransformPoint(rayHits[i].point).z;
                    hitSomething = true;
                }
            }

            if (hitSomething)
            {
                Debug.DrawRay(checkRay.origin, -pivotTransform.forward * (targetDistance + sphereCastRadius), Color.red);
            }

            dodging = hitSomething;
            currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref cameraVelocity, (currentDistance > targetDistance ? moveTime : returnTime));
            currentDistance = Mathf.Clamp(currentDistance, closestDistance, initialDistance);
            cameraTransform.localPosition = -Vector3.forward * currentDistance;
            pivotTransform.localPosition = new Vector3(currentTarget.offset.x, currentTarget.offset.y, 0f);
        }
        else
        {
            pivotTransform.localPosition = new Vector3(currentTarget.offset.x, currentTarget.offset.y, 0f);
            cameraTransform.localPosition = -Vector3.forward * currentTarget.distance;
        }
    }

    public void ManualUpdate()
    {
        if (currentTarget == null && (targets.Length == 0 || AllTargetsDisabled()))
            return;
        if (updateType == UpdateType.ManualUpdate)
            FollowTarget(Time.deltaTime);
    }

    private void FollowTarget(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, currentTarget.target.position, deltaTime * moveSpeed);

        if(cameraTransform.GetComponent<SphereCollider>() != null)
            cameraTransform.GetComponent<SphereCollider>().radius = sphereCastRadius;
    }

    private void HandleRotation()
    {
        if (Time.timeScale < float.Epsilon)
            return;

        float horizontal = Input.GetAxisRaw("Camera X");
        float vertical = Input.GetAxisRaw("Camera Y");

        horizontal *= invertX ? -1f : 1f;
        vertical *= invertY ? -1f : 1f;

        lookAngle += horizontal * turnSpeed;

        if (maximumX != Mathf.Infinity && minimumX != Mathf.NegativeInfinity)
            lookAngle = Mathf.Clamp(lookAngle, minimumX, maximumX);

        transformTargetRotation = Quaternion.Euler(0f, lookAngle, 0f);

        tiltAngle -= vertical * turnSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -minimumY, maximumY);

        pivotTargetRotation = Quaternion.Euler(tiltAngle, pivotEulers.y, pivotEulers.z);

        if(turnSmoothing > 0f)
        {
            pivotTransform.localRotation = Quaternion.Slerp(pivotTransform.localRotation, pivotTargetRotation, turnSmoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transformTargetRotation, turnSmoothing * Time.deltaTime);
        }
        else
        {
            pivotTransform.localRotation = pivotTargetRotation;
            transform.localRotation = transformTargetRotation;
        }
    }

    public void SetTarget(int index)
    {
        if(targets.Length >= (index + 1))
        {
            currentTarget = targets[index];
            initialDistance = currentTarget.distance;
            pivotTransform.localPosition = new Vector3(currentTarget.offset.x, currentTarget.offset.y, 0f);
        }
        else
        {
            Debug.LogWarning("Warning: Something called to switch to a target that doesn't exist.");
        }
    }

    private bool AllTargetsDisabled()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].target.gameObject.activeSelf)
            {
                currentTarget = targets[i];
                targetIndex = i;
                return false;
            }
        }

        return true;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
