using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "UIController/State")]
public class UIState : ScriptableObject {
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
}
