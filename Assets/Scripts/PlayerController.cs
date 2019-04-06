using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float stickToGroundForce = 9.8f;
    [SerializeField] private float gravityMultiplyer = 2f;
    [SerializeField] private float jetpackGravityScale;
    [SerializeField] private float jetJump;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private bool jetPack;
   
    
    private Camera mCamera;
    private bool jump;
    private Vector2 mInput;
    private Vector3 moveDir = Vector3.zero;
    //private Vector3 mForward;
    private CharacterController characterController;
    private CollisionFlags collisionFlags;
    private bool previouslyGrounded;
    private bool jumping;
    private bool isWalking;

    // Health Stuff Kyle Added This
    public GameObject Hud;
    public int Health = 100;
    private HealthBarScript mHealthBar;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
        mCamera = Camera.main;
        jumping = false;
        cameraController.Init(this, mCamera.transform);

        //mForward = mCamera.transform.forward;

        //Get Health Information Kyle Added This
        mHealthBar = Hud.transform.Find("HealthBar").GetComponent<HealthBarScript>();
        mHealthBar.MininumHealth = 0;
        mHealthBar.Maxhealth = Health;
	}

    // Damage Code Added By Kyle
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
        {
            Health = 0;
        }

            mHealthBar.SetHealth(Health);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log(mCamera.transform.parent);
        }

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
        GetInput(out speed);

        Vector3 desiredMove = mCamera.transform.parent.forward * mInput.y + mCamera.transform.parent.right * mInput.x;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;

        if (characterController.isGrounded)
        {
            moveDir.y = -stickToGroundForce;

            if (jump)
            {
                if (jetPack == true)
                {
                    moveDir.y = jetJump;
                    jump = false;
                    jumping = true;
                }
                else
                {
                    moveDir.y = jumpSpeed;
                    jump = false;
                    jumping = true;
                }
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

        isWalking = !Input.GetKey(KeyCode.LeftShift);

        speed = isWalking ? walkSpeed : runSpeed;

        mInput = new Vector2(horizontal, vertical);

        if (mInput.sqrMagnitude > 1f)
            mInput.Normalize();
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
