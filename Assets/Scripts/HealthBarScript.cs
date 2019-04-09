using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    public Image ImageHealthBar;
    public Image ImageDamageBar;
    public Text TextHealth;
    public int MininumHealth = 0;
    public int Maxhealth = 5;

    //Regenerate health variables
    public float RegenRate = 1f;
    public float timer = 0f;
    public float timerDelay = 0f;
    public float healthTimer = 5f;
    public int RegenAmount = 1;
    public bool isRegenerating;
    //to get playercontroller
    public GameObject playerObject;

    private float mCurrentPercent;
    private int mCurrentValue;

    //Ammo Variables
    public Image ImageAmmoBar;
    public Image ImageAmmoOutBar;
    public int MinimumAmmo;
    public int MaxAmmo;

    public float ChargeRate = 1f;
    public int ChargeAmount = 1;

    private int mCurrentAmmo;
    private float mAmmoPercent;



    public void SetHealth(int health)
    {
        if(Maxhealth - MininumHealth == 0)
        {
            mCurrentValue = 0;
            mCurrentPercent = 0;
        }
        else
        {
            mCurrentValue = health;
            mCurrentPercent = (float)mCurrentValue / (float)(Maxhealth - MininumHealth);
        }

        TextHealth.text = string.Format("{0} %", Mathf.RoundToInt(mCurrentPercent * 100));
        //changes the x value of rectangle to increase or decrease health
        ImageHealthBar.transform.localScale = new Vector3(mCurrentPercent, 1, 1);
        StartCoroutine(TakenDamage(mCurrentPercent));
    }

    public void SetAmmo(int ammo) 
    {
        if(MaxAmmo - MinimumAmmo == 0) 
        {
            mCurrentAmmo = 0;
            mAmmoPercent = 0;
        }
        else
        {
            mCurrentAmmo = ammo;
            mAmmoPercent = (float)mCurrentAmmo / (float)(MaxAmmo - MinimumAmmo);
        }

        ImageAmmoBar.transform.localScale = new Vector3(mAmmoPercent, 1, 1);
    }

    public float CurrentPercent
    {
        get { return mCurrentPercent; }
    }

    public int CurrentValue
    {
        get { return mCurrentValue; }
    }

    public int CurrentAmmo {
        get { return mCurrentAmmo; }
    }
    public float AmmoPercent {
        get { return mAmmoPercent; }
    } 

	// Use this for initialization
	void Start () {
        SetHealth(100);
        SetAmmo(100);
        isRegenerating = false;
    }
    //after n seconds player's health will regenerate
    void FixedUpdate() {
        timer += Time.fixedDeltaTime;
        if (timer >= healthTimer) {
            if (mCurrentValue < Maxhealth) {
                isRegenerating = true;
                timer = 0;
                StartCoroutine(Regenerate(playerObject, RegenRate));
            }
            else {
                isRegenerating = false;
            }
        }
    }
    //Delays Damage health bar
    IEnumerator TakenDamage(float percent) {
        yield return new WaitForSeconds(3);
        ImageDamageBar.transform.localScale = new Vector3(percent, 1, 1);
    }
    //Pauses before giving back a little bit of player health
    IEnumerator Regenerate(GameObject playerObject, float regenRate) 
    {
        if (isRegenerating && mCurrentPercent != 0 && mCurrentPercent < 1) 
        {
            
            PlayerController player = playerObject.GetComponent<PlayerController>();
            mCurrentValue += RegenAmount;
            SetHealth(mCurrentValue);
            if(mCurrentValue >= Maxhealth) 
            {
                SetHealth(Maxhealth);
            }
            yield return new WaitForSeconds(regenRate);
        }
    }
}
