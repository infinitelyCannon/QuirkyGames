using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float stickToGroundForce = 9.8f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float jetPackGravityScale;
    [SerializeField] private float jetJump;
    [SerializeField] private float meshRotationSpeed;
    [SerializeField] private bool jetPack;
    [SerializeField] private float shootDelay;

    private Transform mainCamera;
    private Vector3 cameraForward;
    private Vector3 moveVector;
    private bool jump;
    private bool jumping;
    private CharacterController characterController;
    //private CollisionFlags collisionFlags;
    private bool previouslyGrounded;
    private bool isWalking;
    private float turnAmount;
    private Transform meshObject;
    private float shootWait;
    private bool canShoot = true;

    // Health Stuff Kyle Added This
    public GameObject Hud;
    public int Health = 100;
    private HealthBarScript mHealthBar;

    private void Start()
    {
        if (Camera.main != null)
            mainCamera = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");

        characterController = GetComponent<CharacterController>();
        jumping = false;
        meshObject = transform.GetChild(0);
        shootWait = shootDelay;

        //Get Health Information Kyle Added This
        mHealthBar = Hud.transform.Find("HealthBar").GetComponent<HealthBarScript>();
        mHealthBar.MininumHealth = 0;
        mHealthBar.Maxhealth = Health;
        mHealthBar.SetHealth(Health);
    }

    private void Update()
    {
        if (!jump && !jumping)
            jump = Input.GetButtonDown("Jump");

        if(!previouslyGrounded && characterController.isGrounded)
        {
            moveVector.y = 0f;
            jumping = false;
        }

        if (!characterController.isGrounded && !jumping && previouslyGrounded)
            moveVector.y = 0f;

        previouslyGrounded = characterController.isGrounded;

        if (Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f)
        {
            if (Input.GetButtonDown("Shoot") || Input.GetAxisRaw("Shoot") == 1f)
            {
                Shoot(false);
            }
            else if (Input.GetButtonDown("ShootAlt"))
            {
                Shoot(true);
            }
        }
    }

    private void FixedUpdate()
    {
        // I'm using GetAxisRaw now for the pill-shaped player, but may need to use GetAxis for value smoothing after animations are applied.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float speed;
        RaycastHit hitInfo;
        Vector3 desiredMove;

        if (Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f)
            meshObject.GetComponent<MeshRenderer>().enabled = false;
        else if (Input.GetButtonUp("Aim") || Input.GetAxisRaw("Aim") == 0f)
            meshObject.GetComponent<MeshRenderer>().enabled = true;

        if (horizontal != 0f || vertical != 0f)
        {
            //Handle the rotation of the mesh object (re-write if animations handle this themselves)
            meshObject.localRotation = Quaternion.RotateTowards(meshObject.localRotation, Quaternion.Euler(0f, turnAmount * Mathf.Rad2Deg, 0f), meshRotationSpeed * Time.fixedDeltaTime);
        }

        isWalking = !Input.GetButton("Run");
        speed = isWalking ? walkSpeed : runSpeed;

        if (mainCamera != null)
        {
            cameraForward = Vector3.Scale(mainCamera.forward, new Vector3(1f, 0f, 1f)).normalized;
            desiredMove = vertical * cameraForward + horizontal * mainCamera.right;
        }
        else
            desiredMove = vertical * Vector3.forward + horizontal * Vector3.right;

        if (desiredMove.magnitude > 1f)
            desiredMove.Normalize();

        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);
        turnAmount = Mathf.Atan2(desiredMove.x, desiredMove.z);

        moveVector.x = desiredMove.x * speed;
        moveVector.z = desiredMove.z * speed;

        if (characterController.isGrounded)
        {
            moveVector.y = -stickToGroundForce;

            if (jump)
            {
                if (jetPack)
                {
                    moveVector.y = jetJump;
                    jump = false;
                    jumping = true;
                }
                else
                {
                    moveVector.y = jumpSpeed;
                    jump = false;
                    jumping = true;
                }
            }
        }
        else
        {
            if (jumping && characterController.velocity.y < 0 && Input.GetButton("Jump"))
                moveVector += Physics.gravity * jetPackGravityScale * Time.fixedDeltaTime;
            else
                moveVector += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        characterController.Move(moveVector * Time.fixedDeltaTime);
    }

    private void Shoot(bool alt)
    {
        
    }

    // Damage Code Added By Kyle
    public void TakeDamage(int amount)
    {
        Health = mHealthBar.CurrentValue;
        Health -= amount;
        if (Health < 0)
        {
            Health = 0;
        }

        mHealthBar.SetHealth(Health);
        //mHealthBar.timer = 0f;
        //mHealthBar.isRegenerating = false;
    }
}
