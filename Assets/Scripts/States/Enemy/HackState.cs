using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackState : EnemyState {

    private float hackTimer = 0f;
    private float shootTimer = 0f;
    private Transform meshObject;
    private Vector3 destination;
    private Animator animator;
    private GameObject[] enemies;
    private bool allOut = false;

    public override void EnterState()
    {
        hackTimer = 0f;
        meshObject = transform.GetChild(0);
        animator = GetComponentInChildren<Animator>();
        gameObject.tag = "Controlled";
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
            controller.target = enemies[Random.Range(0, enemies.Length - 1)].transform;
        else
        {
            controller.GoToState("PatrolState");
            return;
        }
        controller.StartHack();
        controller.navAgent.isStopped = true;
    }

    public override void UpdateState()
    {
        if(controller.target == null)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
                controller.target = enemies[Random.Range(0, enemies.Length - 1)].transform;
            else
            {
                controller.GoToState("PatrolState");
                return;
            }
        }

        if (!controller.target.gameObject.activeSelf)
        {
            Destroy(controller.target.gameObject);
            controller.target = null;

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
                controller.target = enemies[Random.Range(0, enemies.Length - 1)].transform;
            else
            {
                controller.GoToState("PatrolState");
                return;
            } 
        }

        //Vector2 rand = Random.insideUnitCircle.normalized * controller.stopDistance;
        //destination = new Vector3(rand.x, transform.position.y, rand.y) + controller.target.position;

        meshObject.LookAt(controller.target.transform);

        /*if (controller.navAgent.remainingDistance <= controller.navAgent.stoppingDistance && !controller.navAgent.pathPending)
        {
            while (!controller.navAgent.SetDestination(destination))
            {
                rand = Random.insideUnitCircle.normalized * controller.stopDistance;
                destination = new Vector3(rand.x, transform.position.y, rand.y) + controller.target.position;
            }

        }*/

        hackTimer += Time.deltaTime;
        if (hackTimer >= controller.mindControlTime)
        {
            hackTimer = 0f;
            controller.GoToState("PatrolState");
        }

        shootTimer += Time.deltaTime;
        if(shootTimer >= controller.shootDelay)
        {
            shootTimer = 0f;
            animator.SetTrigger("Shoot");
        }
    }

    public override void ExitState()
    {
        EnemyStateController.traitor = null;
        controller.target = null;
        controller.StopHack();
        gameObject.tag = "Enemy";
        controller.navAgent.isStopped = false;
    }
}
