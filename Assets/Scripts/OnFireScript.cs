using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireScript : MonoBehaviour {

    public GameObject projectile;
    public GameObject mindProjectile;
    public GameObject Hud;
    private HealthBarScript mAmmoBar;
    public int ammoCount = 100;
    public int ammoAmnt = 10;
    //private Rigidbody rb;

    private Vector3 cameraForward;
    private Vector3 desiredDir;

    //Addons from Dakarai
    private PlayerController player;
    public Transform spawnPoint;

    private bool isFiring;

    public bool isEquipped;

    //Recharge Ammo Variables
    public float ammoTimer = 0f;
    public float chargeTimer = 2f;
    private bool canRecharge;
    private bool isOut; //Prevents firing when fully out of ammo

    //Sounds
    public AudioClip projectileClip;
    public AudioClip mindControlClip;
    private AudioSource mAudioSource;


    // Use this for initialization
    void Start () {
        mAmmoBar = Hud.transform.Find("HealthBar").GetComponent<HealthBarScript>();
        mAmmoBar.MinimumAmmo = 0;
        mAmmoBar.MaxAmmo = ammoCount;
        mAmmoBar.SetAmmo(ammoCount);

        isFiring = true;
        canRecharge = false;
        isOut = false;

        mAudioSource = GetComponent<AudioSource>();

        //Addons by Dakarai
        player = gameObject.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetButtonDown("Shoot") || Input.GetAxisRaw("Shoot") == 1f) && isFiring == true && Time.timeScale > 0f) 
        {
            isFiring = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isAiming = (Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f);
                
            FiredShot();
            if (ammoCount > 0 && isOut == false) {
                Instantiate(projectile, player.GetSpawnPosition(isAiming), transform.rotation).transform.forward = player.GetShotDirection(isAiming);
                //cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
                //projectile.transform.forward = player.GetShotDirection();
                mAudioSource.PlayOneShot(projectileClip);
            }   
        }
        else if (Input.GetButtonDown("ShootAlt") && Time.timeScale > 0f && isEquipped)
        {
            bool isAiming = (Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f);

            if (ammoCount > 0)
            {
                Instantiate(mindProjectile, player.GetSpawnPosition(isAiming), transform.rotation).transform.forward = player.GetShotDirection(isAiming);
                mAudioSource.PlayOneShot(mindControlClip);
            }
            //FiredShot();
        }
        if (Input.GetButtonUp("Shoot")) {
            isFiring = true;
        }

        //Timer
        ammoTimer += Time.deltaTime;
        if(ammoTimer >= chargeTimer) {
            canRecharge = true;
            Charge();
            ammoTimer = 0;
        }
	}

    //full variable or == to max
    //canRecharge a bool set by a timer
    //isOut a bool set when ammoCount reaches 0

    private void FiredShot() 
    {
        if (ammoCount > 0 && canRecharge == false && isOut == false) 
        {
            ammoCount = mAmmoBar.CurrentAmmo;
            ammoCount -= ammoAmnt;
            mAmmoBar.SetAmmo(ammoCount);
            ammoTimer = 0;
        }
        if(ammoCount <= 0) {
            ammoCount = 0;
            mAmmoBar.SetAmmo(ammoCount);
            isOut = true;
        }
        
    }
    private void Charge() {
        if (ammoCount <= mAmmoBar.MaxAmmo) 
        {
            ammoCount = mAmmoBar.CurrentAmmo;
            ammoCount += ammoAmnt;
            mAmmoBar.SetAmmo(ammoCount);
        }
        if(ammoCount >= mAmmoBar.MaxAmmo) {
            ammoCount = mAmmoBar.MaxAmmo;
            mAmmoBar.SetAmmo(ammoCount);
            isOut = false;
        }
        canRecharge = false;
    }

    private void PlayProjectileSound() {
        mAudioSource.clip = projectileClip;
        mAudioSource.Play();
    }

    private void PlayMindControlSound() {
        mAudioSource.clip = mindControlClip;
        mAudioSource.Play();
    }

}
