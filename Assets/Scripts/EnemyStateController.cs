using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour {

    public float speed;
    public float stopDistance;
    public float shootDelay;
    public GameObject projectile;
    public GameObject enemyProjectile;
    public float mindControlTime;
    public float retreatDistance;
    public int patrolStart;
    public Transform[] patrolSpots;
    public float hoverHeight;
    public float hoverSpeed;
    public GameObject explosion;
    public GameObject hackContainer;
    public AudioClip takeShot;

    public static EnemyStateController traitor = null;
    [HideInInspector] public bool playerInRange = false;
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Transform target;

    private EnemyState[] states;
    private int currentState = -1;
    private Transform meshObject;
    private float hoverTime = 0f;
    private ParticleSystem[] hacks;
    private Transform player;
    private AudioSource source;

	// Use this for initialization
	void Start () {
        states = GetComponents<EnemyState>();
        meshObject = transform.GetChild(0);
        hacks = hackContainer.GetComponentsInChildren<ParticleSystem>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        source = GetComponent<AudioSource>();
        StopHack();
        GoToState("PatrolState");
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale > 0f && currentState != -1)
        {
            states[currentState].UpdateState();
        }

        hoverTime += Time.deltaTime;
        meshObject.localPosition += new Vector3(0f, (Mathf.Sin(hoverTime * hoverSpeed) * hoverHeight) * Time.deltaTime, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            ScoreScript.instance.AddToScore(100);
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("FriendlyFire") && gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            ScoreScript.instance.AddToScore(150);
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("MindBullet") && traitor == null)
        {
            traitor = this;
            GoToState("HackState");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public void GoToState(string name)
    {
        if (currentState != -1)
            states[currentState].ExitState();

        for(int i = 0; i < states.Length; i++)
        {
            if(states[i].ClassName() == name)
            {
                currentState = i;
                states[i].EnterState();
                return;
            }
        }

        currentState = -1;
        Debug.LogWarning("Enemy Controller called to enter unknown state: " + "\"" + name + "\"");
    }

    public void Fire()
    {
        GameObject bullet;

        if (traitor == null)
        {
            source.PlayOneShot(takeShot);
            bullet = Instantiate(projectile, transform.position + meshObject.forward, Quaternion.identity);
            bullet.transform.LookAt(player);
        }
        else
        {
            source.PlayOneShot(takeShot);
            bullet = Instantiate(enemyProjectile, transform.position + meshObject.forward, Quaternion.identity);
            bullet.transform.LookAt(target);
        }
    }

    public void StartHack()
    {
        foreach(ParticleSystem h in hacks)
        {
            h.Play();
        }
    }

    public void StopHack()
    {
        foreach(ParticleSystem h in hacks)
        {
            h.Stop();
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject, 1.5f);
    }
}
