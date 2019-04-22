using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathScript : MonoBehaviour {
    public GameObject malfunction;



	// Use this for initialization
	void Start () {
        Explode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //Disable enemy movement on death then have them fade out
    public void Explode() {
        if(malfunction != null) {
            GameObject malfunctionFx = Instantiate(malfunction, transform.position, Quaternion.identity);
            Destroy(malfunctionFx, 5);
        }
    }
}
