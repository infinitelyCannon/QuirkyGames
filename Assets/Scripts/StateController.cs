using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StateController : MonoBehaviour {

    public delegate void AfterEffect();
    public enum TriggerEvent
    {
        Axis,
        Submit,
        Cancel
    };
    public enum TypeAction
    {
        Clear,
        Append,
        Replace
    };

    public enum OutSideUIAction
    {
        Play,
        Quit,
        Close
    };

    public float blinkTime;
    public float typeSpeed;
    public UIState currentState;
    public UIState noOp;

    private Text mText;
    private const char BOX = '▁';
    private int lastIndex = 0;
    private bool isReady = false;
    private bool shouldFlush = false;
    private bool blinkOn = true;
    private EventSystem eventSystem;

    // Use this for initialization
    void Start () {
        mText = transform.GetComponentInChildren<Text>();
        mText.text = BOX.ToString();
        StartCoroutine("Blink");
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Move;
        entry.callback.AddListener((data) => { MoveTrigger((AxisEventData)data); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry submitEntry = new EventTrigger.Entry();
        submitEntry.eventID = EventTriggerType.Submit;
        submitEntry.callback.AddListener((data) => { SubmitTrigger(data); });
        trigger.triggers.Add(submitEntry);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private int FindBrick()
    {
        for (int i = 0; i < mText.text.Length; i++)
        {
            if (mText.text[i].Equals(BOX))
            {
                lastIndex = i;
                return i;
            }

        }

        return lastIndex;
    }

    public void MoveTrigger(AxisEventData data)
    {

    }

    public void SubmitTrigger(BaseEventData data)
    {

    }

    public void TransitionToState(UIState nextState)
    {

    }

    IEnumerator Blink()
    {
        char[] temp;
        int boxIndex = -1;

        while (true)
        {
            temp = mText.text.ToCharArray();
            boxIndex = FindBrick();

            if (blinkOn)
                temp[boxIndex] = ' ';
            else
                temp[boxIndex] = BOX;

            blinkOn = !blinkOn;
            mText.text = new string(temp);
            yield return new WaitForSeconds(blinkTime);
        }
    }
}
