using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        transform.GetChild(0).Rotate(new Vector3(-5f, 0f, 0f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("POW");
            Destroy(gameObject);
        }
        */
        Destroy(gameObject);
    }

    /*
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
    }
    */
}
