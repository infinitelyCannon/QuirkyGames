using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIState : MonoBehaviour {

    protected static EventSystem eventSystem;

    protected virtual void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public abstract void UpdateState();
    public abstract void EnterState(PauseMenuScript pauseMenu);
    public abstract void ExitState();

    /*
    public UIAction[] actions;
    public UITransition transition;
    public float startDelay;
    public float endDelay;
    public StateController.TypeAction actionType;
    [TextArea(maxLines: 20, minLines: 10)] public string text;

    private bool flush = false;
    private float pause = 0f;

    public void UpdateState(StateController controller)
    {
        switch (actionType)
        {
            case StateController.TypeAction.Append:
            case StateController.TypeAction.Clear:

                break;
            case StateController.TypeAction.Replace:
                break;
            default:
                break;
        }
    }

    public void HandleEvent(BaseEventData data, StateController.TriggerEvent triggerEvent)
    {

    }
    */
}
