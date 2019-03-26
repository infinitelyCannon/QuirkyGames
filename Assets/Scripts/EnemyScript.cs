using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public float speed;
    public float stopDistance;
    public float retreatDistance;

    public bool inRadius;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    public Transform player;
    public Transform enemyMind;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyMind = GameObject.FindGameObjectWithTag("Enemy").transform;
        timeBtwShots = startTimeBtwShots;
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinZone();
        }
    }

    // Update is called once per frame
    void PlayerinZone ()
    {
        //Enemy Movement
		if (Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, player.position) < stopDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

       



        if (timeBtwShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }



}

