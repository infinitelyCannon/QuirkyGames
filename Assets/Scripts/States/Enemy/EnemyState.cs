using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour {
    protected EnemyStateController controller;

    protected virtual void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();


    public string ClassName()
    {
        string result = ToString();

        return result.Substring(result.LastIndexOf('(') + 1, result.LastIndexOf(')') - (result.LastIndexOf('(') + 1));
    }
}
