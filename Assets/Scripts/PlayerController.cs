﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float hoverHeight;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private GameObject JetPack;
    public ParticleSystem jetpack;
    public GameObject hoverContainer;
    public GameObject coreContainer;

    private ParticleSystem[] hoverSet;
    private ParticleSystem[] coreSet;
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
    private float hoverTime = 0f;
    private Animator animator;

    // Health Stuff Kyle Added This
    public GameObject Hud;
    public int Health = 100;
    private HealthBarScript mHealthBar;

    //Vertical Slice stuff
    public PauseMenuScript deathScreen;
    private bool isDead = false;
    public Transform bulletPoint;
    private bool done = false;

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
        hoverSet = hoverContainer.GetComponentsInChildren<ParticleSystem>();
        coreSet = coreContainer.GetComponentsInChildren<ParticleSystem>();

        //Get Health Information Kyle Added This
        mHealthBar = Hud.transform.Find("HealthBar").GetComponent<HealthBarScript>();
        mHealthBar.MininumHealth = 0;
        mHealthBar.Maxhealth = Health;
        mHealthBar.SetHealth(Health);

        animator = transform.GetChild(0).GetComponent<Animator>();
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

        if(characterController.isGrounded && !hoverSet[0].isPlaying && !(Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f))
        {
            for (int i = 0; i < hoverSet.Length; i++)
                hoverSet[i].Play();
        }

        //Vertical Slice stuff
        if(Health <= 0 && !isDead)
        {
            isDead = true;
            deathScreen.Launch();
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
        {
            meshObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;//.GetComponent<MeshRenderer>().enabled = false;
            foreach(ParticleSystem ps in coreSet)
            {
                ps.Stop();
            }
            foreach(ParticleSystem hover in hoverSet)
            {
                hover.Stop();
            }
        }

        else if (Input.GetButtonUp("Aim") || Input.GetAxisRaw("Aim") == 0f)
        {
            meshObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true; ;//.GetComponent<MeshRenderer>().enabled = true;
            foreach (ParticleSystem ps in coreSet)
            {
                ps.Play();
            }
            foreach (ParticleSystem hover in hoverSet)
            {
                hover.Play();
            }
        }

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

        // Vertical Slice Stuff
        //Debug.DrawRay(bulletPoint.position, meshObject.forward * 3f, Color.cyan, 0.1f);

        moveVector.x = desiredMove.x * speed;
        moveVector.z = desiredMove.z * speed;

        if (characterController.isGrounded)
        {
            moveVector.y = -stickToGroundForce;

            if (jump)
            {
                meshObject.localPosition = new Vector3(0f, -1f, 0f);
                animator.SetTrigger("Jump");
                for (int i = 0; i < hoverSet.Length; i++)
                    hoverSet[i].Stop();
                hoverTime = 0f;
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
            else
            {
                hoverTime += Time.deltaTime;
                meshObject.localPosition += new Vector3(0f, (Mathf.Sin(hoverTime * hoverSpeed) * hoverHeight) * Time.deltaTime, 0f);
            }
        }
        else
        {
            if (jumping && characterController.velocity.y < 0 && Input.GetButton("Jump") && jetPack)
                moveVector += Physics.gravity * jetPackGravityScale * Time.fixedDeltaTime;
            else
                moveVector += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        characterController.Move(moveVector * Time.fixedDeltaTime);

        // Handle Animations
        animator.SetBool("Moving", (horizontal != 0f || vertical != 0f));
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width - 100, Screen.height - 20, 100, 20), Mathf.Atan2(moveVector.x, moveVector.z).ToString());
    }

    public Vector3 GetShotDirection(bool aiming)
    {
        if (aiming)
            return mainCamera.forward;// + Vector3.up;//Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 1));
        else
            return meshObject.forward;
    }

    public Vector3 GetSpawnPosition(bool aiming)
    {
        if (aiming)
            return mainCamera.position;// + Vector3.down;
        else
            return bulletPoint.position;
    }

    private void OnTriggerEnter(Collider other)
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
        mHealthBar.timer = 0f;
        mHealthBar.isRegenerating = false;
    }

    public void ActivateJetPack()
    {
        jetPack = true;
        JetPack.SetActive(true);
    }
}
