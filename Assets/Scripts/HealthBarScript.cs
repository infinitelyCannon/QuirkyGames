using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    public Image ImageHealthBar;
    public Text TextHealth;
    public int MininumHealth;
    public int Maxhealth;



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
        ImageHealthBar.fillAmount = mCurrentPercent;
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
	}
	

}
