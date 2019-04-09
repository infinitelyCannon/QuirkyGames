using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireScript : MonoBehaviour {

    public GameObject projectile;
    public GameObject Hud;
    private HealthBarScript mAmmoBar;
    public int ammoCount = 100;
    //private Rigidbody rb;

    private Vector3 cameraForward;
    private Vector3 desiredDir;


    // Use this for initialization
    void Start () {
        mAmmoBar = Hud.transform.Find("HealthBar").GetComponent<HealthBarScript>();
        mAmmoBar.MinimumAmmo = 0;
        mAmmoBar.MaxAmmo = ammoCount;
        mAmmoBar.SetAmmo(ammoCount);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Shoot")) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            FiredShot();
            if (ammoCount > 0) {
                Instantiate(projectile, transform.position, transform.rotation);
                /*cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
                projectile.transform.forward = cameraForward;*/
            }   
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
