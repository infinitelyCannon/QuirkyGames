using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UITransition {
    //public UIDecision decision;
    public UIState upState;
    public UIState downState;
    public UIState leftState;
    public UIState rightState;
    public UIState submitState;
    public UIState cancelState;
}
