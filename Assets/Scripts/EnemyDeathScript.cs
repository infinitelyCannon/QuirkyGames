using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathScript : MonoBehaviour {
    public GameObject malfunction;
    public GameObject explosion;
    public Vector3 offsetFx;



	// Use this for initialization
	void Start () {
        offsetFx = new Vector3(0, 1, 0);
        Explode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //Disable enemy movement on death then have them fade out
    public void Explode() {
        if(malfunction != null) {
            GameObject malfunctionFx = Instantiate(malfunction, transform.position + offsetFx, Quaternion.identity);
            GameObject explosionFx = Instantiate(explosion, transform.position + offsetFx, Quaternion.identity);
            Destroy(malfunctionFx, 5);
            Destroy(explosionFx, 5);
        }
    }
}
