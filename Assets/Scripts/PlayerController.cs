using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [SerializeField] private bool showCameraSphere = true;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float stickToGroundForce = 9.8f;
    [SerializeField] private float gravityMultiplyer = 2f;
    [SerializeField] private float jetpackGravityScale;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private float meshRotationSpeed;

    public GameObject winPanel;

    private Camera mCamera;
    private bool jump;
    private Vector2 mInput;
    private Vector3 moveDir = Vector3.zero;
    private CharacterController characterController;
    private CollisionFlags collisionFlags;
    private bool previouslyGrounded;
    private bool jumping;
    private bool isWalking;
    private float lerpIn = 0f;
    private float lerpOut = 0f;
    private float originalDistance;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
        mCamera = Camera.main;
        jumping = false;
        cameraController.Init(this, mCamera.transform);
        originalDistance = cameraController.distance;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (transform.position.y <= -17f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        cameraController.UpdatePosition();
        cameraController.UpdateCursorLock();

        if (!jump && !jumping)
            jump = Input.GetButtonDown("Jump");

        if(!previouslyGrounded && characterController.isGrounded)
        {
            moveDir.y = 0f;
            jumping = false;
        }

        if (!characterController.isGrounded && !jumping && previouslyGrounded)
            moveDir.y = 0f;

        previouslyGrounded = characterController.isGrounded;
	}

    private void FixedUpdate()
    {
        float speed;
        //May need to modifiy this bitmask later to ignore things like particle effects or projectiles.
        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        GetInput(out speed);

        Vector3 desiredMove = mCamera.transform.forward.normalized * mInput.y + mCamera.transform.right.normalized * mInput.x;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        if (cameraController.checkForCollision)
        {
            RaycastHit cameraRay;
            if (Physics.Raycast(transform.position, mCamera.transform.forward * -1f, out cameraRay, cameraController.distance, layerMask))
            {
                lerpOut = 0f;
                if (cameraRay.distance >= cameraController.minDistance)
                {
                    cameraController.distance = Mathf.Lerp(cameraController.distance, cameraRay.distance, lerpIn);
                    lerpIn += 0.08f * Time.fixedDeltaTime;
                }
                else
                {
                    cameraController.distance = Mathf.Lerp(cameraController.distance, cameraController.minDistance, lerpIn);
                    lerpIn += 0.2f * Time.fixedDeltaTime;
                }
            }
            else
            {
                lerpIn = 0f;
                if (cameraController.distance != originalDistance)
                {
                    cameraController.distance = Mathf.Lerp(cameraController.distance, originalDistance, lerpOut);
                    lerpOut += 0.2f * Time.fixedDeltaTime;
                }
            }
        }

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;

        if (characterController.isGrounded)
        {
            moveDir.y = -stickToGroundForce;

            if (jump)
            {
                moveDir.y = jumpSpeed;
                jump = false;
                jumping = true;
            }
        }
        else
        {
            if (jumping && characterController.velocity.y < 0 && Input.GetButton("Jump"))
                moveDir += Physics.gravity * jetpackGravityScale * Time.fixedDeltaTime;
            else
                moveDir += Physics.gravity * gravityMultiplyer * Time.fixedDeltaTime;
        }

        collisionFlags = characterController.Move(moveDir * Time.fixedDeltaTime);
    }

    private void GetInput(out float speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Transform mesh = transform.GetChild(0);
        float angle;

        if (horizontal != 0f || vertical != 0f)
        {
            angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            if (inRange(0, 90, angle))
                angle = LinearMap(0f, 90f, 90f, 0f, angle);
            else if (inRange(-90f, 0f, angle))
                angle = LinearMap(0f, -90f, 90f, 180f, angle);
            else if (inRange(90f, 180f, angle))
                angle = LinearMap(90f, 180f, 0f, -90f, angle);
            else if (inRange(-180f, 0f, angle))
                angle = LinearMap(-180f, -90f, -90f, -180f, angle);

            mesh.localRotation = Quaternion.RotateTowards(mesh.localRotation, Quaternion.Euler(0f, angle, 0f), meshRotationSpeed * Time.deltaTime);
        }

        isWalking = !Input.GetButton("Run");

        speed = isWalking ? walkSpeed : runSpeed;

        mInput = new Vector2(horizontal, vertical);

        if (mInput.sqrMagnitude > 1f)
            mInput.Normalize();
    }

    IEnumerator LoadAfterDelay(float secs)
    {
        yield return new WaitForSeconds(secs);
        SceneManager.LoadScene(1);
    }

    private float UnitAngleInDeg(Vector3 position)
    {
        Vector3 unitVector = Vector3.Normalize(position);
        float refAngle = Mathf.Atan(unitVector.z / unitVector.x) * Mathf.Rad2Deg;

        if (unitVector.z >= 0f && unitVector.x >= 0f)
            return refAngle;
        else if (unitVector.z >= 0f && unitVector.x < 0f)
            return 180f - Mathf.Abs(refAngle);
        else if (unitVector.z < 0f && unitVector.x < 0f)
            return 180f + Mathf.Abs(refAngle);
        else
            return 360f - Mathf.Abs(refAngle);
    }

    private float LinearMap(float startIn, float endIn, float startOut, float endOut, float value)
    {
        return (value - startIn) * ((endOut - startOut) / (endIn - startIn)) + startOut;
    }

    private bool inRange(float min, float max, float value)
    {
        if (value < min || value > max)
            return false;
        else
            return true;
    }

    private void OnDrawGizmos()
    {
        if (showCameraSphere)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, cameraController.distance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if(winPanel != null)
            {
                winPanel.transform.localScale = Vector3.one;
                StartCoroutine(LoadAfterDelay(5f));
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (collisionFlags == CollisionFlags.Below)
            return;
        if (body == null || body.isKinematic)
            return;

        body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
