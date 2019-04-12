using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireScript : MonoBehaviour {

    public GameObject projectile;
    public GameObject mindProjectile;
    public GameObject Hud;
    private HealthBarScript mAmmoBar;
    public int ammoCount = 100;
    //private Rigidbody rb;

    private Vector3 cameraForward;
    private Vector3 desiredDir;

    //Addons from Dakarai
    private PlayerController player;
    public Transform spawnPoint;


    // Use this for initialization
    void Start () {
        mAmmoBar = Hud.transform.Find("HealthBar").GetComponent<HealthBarScript>();
        mAmmoBar.MinimumAmmo = 0;
        mAmmoBar.MaxAmmo = ammoCount;
        mAmmoBar.SetAmmo(ammoCount);

        //Addons by Dakarai
        player = gameObject.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Shoot") || Input.GetAxisRaw("Shoot") == 1f) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isAiming = (Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f);
                
            FiredShot();
            if (ammoCount > 0) {
                Instantiate(projectile, spawnPoint.position, transform.rotation).transform.forward = player.GetShotDirection(isAiming);
                //cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
                //projectile.transform.forward = player.GetShotDirection();
            }   
        }
        else if (Input.GetButtonDown("ShootAlt"))
        {
            bool isAiming = (Input.GetButton("Aim") || Input.GetAxisRaw("Aim") == 1f);

            if (ammoCount > 0)
            {
                Instantiate(mindProjectile, spawnPoint.position, transform.rotation).transform.forward = player.GetShotDirection(isAiming);
            }
            FiredShot();
        }
	}

    private void FiredShot() 
    {
        if (ammoCount > 0) 
        {
            ammoCount = mAmmoBar.CurrentAmmo;
            ammoCount -= 5;
            mAmmoBar.SetAmmo(ammoCount);
        }
        else {
            ammoCount = 0;
            mAmmoBar.SetAmmo(ammoCount);
        }
        
    }

}
