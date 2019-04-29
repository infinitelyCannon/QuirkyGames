using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState {

    private Transform player;
    private Transform meshObject;
    private Vector3 dest;
    private float shootTimer;
    private Animator animator;

    public override void EnterState()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        meshObject = transform.GetChild(0);
        shootTimer = 0f;
        animator = GetComponentInChildren<Animator>();
        controller.navAgent.isStopped = true;
    }

    public override void UpdateState()
    {
        //Vector2 rand = Random.insideUnitCircle.normalized * controller.stopDistance;
        //dest = new Vector3(rand.x, transform.position.y, rand.y) + player.position;

        meshObject.LookAt(player);

        /*
        if (controller.navAgent.remainingDistance <= controller.navAgent.stoppingDistance && !controller.navAgent.pathPending)
        {
            while (!controller.navAgent.SetDestination(dest))
            {
                rand = Random.insideUnitCircle.normalized * controller.stopDistance;
                dest = new Vector3(rand.x, transform.position.y, rand.y) + player.position;
            }

        }
        */

        shootTimer += Time.deltaTime;
        if(shootTimer >= controller.shootDelay)
        {
            shootTimer = 0f;
            animator.SetTrigger("Shoot");
        }

        if (Vector3.Distance(transform.position, player.position) >= controller.retreatDistance)
            controller.GoToState("PatrolState");
    }

    public override void ExitState()
    {
        controller.navAgent.isStopped = false;
    }
}
