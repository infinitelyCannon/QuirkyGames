using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    public Image ImageHealthBar;
    public Image ImageDamageBar;
    public Text TextHealth;
    public int MininumHealth;
    public int Maxhealth;

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

    public float CurrentPercent
    {
        get { return mCurrentPercent; }
    }

    public int CurrentValue
    {
        get { return mCurrentValue; }
    }

	// Use this for initialization
	void Start () {
        SetHealth(100);
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
