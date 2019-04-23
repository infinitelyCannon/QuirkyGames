using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootTrigger : MonoBehaviour {

    private EnemyScript enemyScript;

	// Use this for initialization
	void Start () {
        enemyScript = GetComponentInParent<EnemyScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        enemyScript.Fire();
    }
}
