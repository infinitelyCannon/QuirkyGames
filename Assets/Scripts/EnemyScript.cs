using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {

    private enum AiState
    {
        Patroling,
        Fighting,
        Hacked
    }

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
    public static EnemyScript traitor = null;
    private float timeControlled;
    public float starttimeControlled;

    //Alpha Stuff
    private NavMeshAgent navAgent;
    public int patrolStart;
    public Transform[] patrolSpots;
    private int patrolIndex;
    private Transform meshObject;
    private float hoverTime = 0f;
    public float hoverHeight;
    public float hoverSpeed;
    private AiState state = AiState.Patroling;
    private float turnAmount;
    private bool isMoving = false;
    public GameObject explosion;
    [HideInInspector] public Transform target;
    public Animator animator;
    public GameObject hackContainer;
    private ParticleSystem[] hacks;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        timeBtwShots = startTimeBtwShots;
        timeControlled = starttimeControlled;
        mindControl = false;
        navAgent = GetComponent<NavMeshAgent>();
        patrolIndex = patrolStart;
        meshObject = transform.GetChild(0);
        navAgent.SetDestination(patrolSpots[patrolIndex].position);
        navAgent.updateRotation = false;
        hacks = hackContainer.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem h in hacks)
        {
            h.Stop();
        }
    }

    void Update()
    {
        // Sets the mind control back to normal
        /*if (traitor == this)
        {
            
            timeControlled -= Time.deltaTime;
            if (timeControlled <= 0)
            {
                traitor = null;
                mindControl = false;
                timeControlled = starttimeControlled;
                transform.gameObject.tag = "Enemy";
            }
            
        }*/

        

        hoverTime += Time.deltaTime;
        meshObject.localPosition += new Vector3(0f, (Mathf.Sin(hoverTime * hoverSpeed) * hoverHeight) * Time.deltaTime, 0f);
    }

    public void Fire()
    {
        if(target == null)
            Instantiate(projectile, transform.position + meshObject.forward, Quaternion.identity);
        else
            Instantiate(enemyProjectile, transform.position + meshObject.forward, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            ScoreScript.instance.AddToScore(100);
            Destroy(gameObject);
        }

        // MindControlling only one enemy at a time
        if (collision.gameObject.CompareTag("MindBullet") && traitor == null)
        {
            traitor = this;
            mindControl = true;
            transform.gameObject.tag = "Controlled";
            state = AiState.Hacked;
        }

        if (collision.gameObject.CompareTag("FriendlyFire") && gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            ScoreScript.instance.AddToScore(150);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && traitor != this)
            state = AiState.Fighting;

        /*else if (other.CompareTag("Bullet"))
        {
            
            Destroy(gameObject);
        } */
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.CompareTag("Player") && mindControl == false)
        {
             PlayerinZone();
        }
        //MindControl Stuff
        else if (other.CompareTag("Enemy") && mindControl == true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int i = Random.Range(0, enemies.Length);

            enemyMind = enemies[i].transform; //GameObject.FindGameObjectWithTag("Enemy").transform;

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


          

        }*/
    }

    
    void PlayerinZone ()
    {
        /*//Enemy Movement
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
        }*/
    }



 
}

