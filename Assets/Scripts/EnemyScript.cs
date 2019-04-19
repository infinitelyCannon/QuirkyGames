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

    //Mind Control one enemy
    private static EnemyScript traitor = null;
    private float timeControlled;
    public float starttimeControlled;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        timeBtwShots = startTimeBtwShots;
        timeControlled = starttimeControlled;
        mindControl = false;
        gameObject.GetComponent<MeshRenderer>().material = normal;
        
        
	}

    void Update()
    {
        // Sets the mind control back to normal
        if (traitor == this)
        {
            
            timeControlled -= Time.deltaTime;
            if (timeControlled <= 0)
            {
                traitor = null;
                mindControl = false;
                timeControlled = starttimeControlled;
                gameObject.GetComponent<MeshRenderer>().material = normal;
            }
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // MindControlling only one enemy at a time
        if (other.CompareTag("MindBullet") && traitor == null)
        {
            traitor = this;
            mindControl = true;
            gameObject.GetComponent<MeshRenderer>().material = mind;
            transform.gameObject.tag = "Controlled";
            

        }

        /*else if (other.CompareTag("Bullet"))
        {
            
            Destroy(gameObject);
        } */
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
            enemyMind = GameObject.FindGameObjectWithTag("Enemy").transform;

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

