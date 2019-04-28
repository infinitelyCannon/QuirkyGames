using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState {
    private int patrolIndex;
    private float turnAmount;
    private Transform meshObject;

    public override void EnterState()
    {
        patrolIndex = controller.patrolStart;
        meshObject = transform.GetChild(0);
        controller.navAgent.updateRotation = false;
    }

    public override void UpdateState()
    {
        controller.navAgent.isStopped = true;
        controller.navAgent.destination = controller.patrolSpots[patrolIndex].position;
        controller.navAgent.isStopped = false;

        if (controller.navAgent.remainingDistance <= controller.navAgent.stoppingDistance && !controller.navAgent.pathPending)
            patrolIndex = ++patrolIndex % controller.patrolSpots.Length;

        turnAmount = Mathf.Atan2(controller.navAgent.velocity.x, controller.navAgent.velocity.z);
        meshObject.localRotation = Quaternion.RotateTowards(meshObject.localRotation, Quaternion.Euler(0f, turnAmount * Mathf.Rad2Deg, 0f), 360f * Time.deltaTime);

        if (controller.playerInRange)
            controller.GoToState("AttackState");
    }

    public override void ExitState()
    {
        return;
    }
}
