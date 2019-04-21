using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIState : MonoBehaviour {

    protected static EventSystem eventSystem;

    protected virtual void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public abstract void UpdateState();
    public abstract void EnterState(PauseMenuScript pauseMenu);
    public abstract void ExitState();
}
