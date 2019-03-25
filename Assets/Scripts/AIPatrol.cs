using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrol : MonoBehaviour {
    //private Character Animation enemyAnim;
    private NavMeshAgent navAgent;

    public float patrol_Radius = 10f;
    public float patrol_Timer = 3f;
    private float timer_Count;

    private Vector3 startPos;

    void Awake() {
        //enemyAnim = GetComponent<CharacterAnimation>();
        navAgent = GetComponent<NavMeshAgent>();

    }

    // Populate waypoints in a small radius on start to avoid going into other enemies patrol.
    void Start () {
        timer_Count = patrol_Timer;
        startPos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        //if (playerSpotted == false) {
            Patrol();
        //}
	}

    void Patrol() {
        timer_Count += Time.deltaTime;

        if(timer_Count > patrol_Timer) {
            SetNewRandomDestination();
            timer_Count = 0f;
        }

        /*if(navAgent.velocity.sqrMagnitude == 0) {
            enemyAnim.Walk(false);
        }
        else {
            enemyAnim.Walk(true);
        }*/
    }

    void SetNewRandomDestination() {
        Vector3 newDestination = RandomNavSphere(startPos, patrol_Radius, -1);
        navAgent.SetDestination(newDestination);
    }

    Vector3 RandomNavSphere(Vector3 originPos, float radius, int layerMask) {
        Vector3 randDir = Random.insideUnitSphere * radius;
        randDir += originPos;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, radius, layerMask);

        return navHit.position;

    }
}
