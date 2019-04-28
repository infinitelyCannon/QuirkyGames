using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootTrigger : MonoBehaviour {

    private EnemyStateController enemyScript;

	// Use this for initialization
	void Start () {
        enemyScript = GetComponentInParent<EnemyStateController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        enemyScript.Fire();
    }
}
