using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public float speed;
    public float stopDistance;
    public float retreatDistance;

    public bool inRadius;
    public bool mindControl;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    // Projectile that is fired if under MindControl
    public GameObject enemyProjectile;
    private Transform player;
    //Get Enemy Transform for MindControl
    private Transform enemyMind;

    //Vertical slice stuff
    public Material normal;
    public Material mind;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyMind = GameObject.FindGameObjectWithTag("Enemy").transform;
        timeBtwShots = startTimeBtwShots;
        mindControl = false;
        gameObject.GetComponent<MeshRenderer>().material = normal;
	}
    private void Update()
    {
        //Switches to MindControlFunctionality
        if (Input.GetKeyDown(KeyCode.M))
        {
            mindControl = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MindBullet"))
        {
            mindControl = true;
            gameObject.GetComponent<MeshRenderer>().material = mind;
        }

        else if (other.CompareTag("Bullet"))
        {
            player.gameObject.SendMessage("AddToScore", 200);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && mindControl == false)
        {
            
            PlayerinZone();
        }
        //MindControl Stuff
        else if (other.CompareTag("Enemy") && mindControl == true)
        {
            if (Vector3.Distance(transform.position, enemyMind.position) > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyMind.position, speed * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, enemyMind.position) < stopDistance && Vector3.Distance(transform.position, enemyMind.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector3.Distance(transform.position, enemyMind.position) < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyMind.position, -speed * Time.deltaTime);
            }

            if (timeBtwShots <= 0)
            {
                Instantiate(enemyProjectile, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }

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

    void MindControl(Collider other)
    {
        //Enemy Movement
        if (other.CompareTag("Enemy"))
        {


            if (Vector3.Distance(transform.position, enemyMind.position) > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyMind.position, speed * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, enemyMind.position) < stopDistance && Vector3.Distance(transform.position, enemyMind.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector3.Distance(transform.position, enemyMind.position) < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyMind.position, -speed * Time.deltaTime);
            }

        }

    }


}

