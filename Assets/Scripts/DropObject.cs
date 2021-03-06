﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
    
   {private Rigidbody rb;
    public OnFireScript mOnFire;

    private bool ready = false;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        //Enable Gravity
        if(other.CompareTag("Bullet") == true) {
            rb.isKinematic = false;
            ready = true;
        }
        //Enable Mind Control
        if(other.CompareTag("Player") == true && gameObject.CompareTag("MacGuffin") == true && ready) {
            mOnFire.isEquipped = true;
            Destroy(gameObject);
            //Add to Player Model
            //Play Nauzik Explaining Mind Control
        }
        
    }
}
