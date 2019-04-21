using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnterScoreState : UIState {

    public Text finalScore;
    public Text scoreTable;
    public int maxNameLength;
    public int maxTabLength = 8;
    public Text nameField;
    public float blinkTime;

    private const string PLAYER_NAME_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_- ";
    private const char BOX = '▁';
    private int currentIndex = 0;
    private int alphabetIndex = PLAYER_NAME_CHARS.Length - 1;
    private bool isOn = false;
    //private char currentLetter = ' ';
    private PauseMenuScript menuScript;

    public override void UpdateState()
    {
        if (gameObject.activeSelf && eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(nameField.gameObject);
        }

        if(Input.inputString != "" && gameObject.activeSelf)
        {
            string input = Input.inputString;
            char[] temp = nameField.text.ToCharArray();
            StopBlink();

            for (int i = 0; i < input.Length; i++)
            {
                if (isValid(input[i]))
                {
                    temp[currentIndex] = input[i];
                    currentIndex = WrapInt(currentIndex + 1, 0, maxNameLength - 1);
                    alphabetIndex = PLAYER_NAME_CHARS.IndexOf(temp[currentIndex]);
                }
                else if (input[i] == '\b')
                {
                    temp[currentIndex] = ' ';
                    if (currentIndex > 0)
                        currentIndex = WrapInt(currentIndex - 1, 0, maxNameLength - 1);
                    alphabetIndex = PLAYER_NAME_CHARS.IndexOf(temp[currentIndex]);
                }
            }

            nameField.text = new string(temp);
            StartCoroutine("Blink");
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button2) && gameObject.activeSelf)
        {
            char[] temp = nameField.text.ToCharArray();
            StopBlink();

            temp[currentIndex] = ' ';
            if (currentIndex > 0)
                currentIndex = WrapInt(currentIndex - 1, 0, maxNameLength - 1);
            alphabetIndex = PLAYER_NAME_CHARS.IndexOf(temp[currentIndex]);
        }
    }

    public override void EnterState(PauseMenuScript pauseMenu)
    {
        menuScript = pauseMenu;
        finalScore.text = ScoreScript.instance.score.ToString() + " Points";
        scoreTable.text = ScoreScript.instance.PrintScores();
        eventSystem.SetSelectedGameObject(nameField.gameObject);
        for (int i = 1; i <= maxNameLength; i++)
            nameField.text += " ";
        StartCoroutine("Blink");
    }

    public override void ExitState()
    {
        StopBlink();
        menuScript = null;
    }

    public void MoveTrigger(BaseEventData data)
    {
        char[] temp = nameField.text.ToCharArray();
        AxisEventData axisData = (AxisEventData) data;

        if (Input.inputString == "")
        {
            StopBlink();
            switch (axisData.moveDir)
            {
                case MoveDirection.Up:
                    alphabetIndex = WrapInt(alphabetIndex + 1, 0, PLAYER_NAME_CHARS.Length - 1);
                    temp[currentIndex] = PLAYER_NAME_CHARS[alphabetIndex];
                    break;
                case MoveDirection.Down:
                    alphabetIndex = WrapInt(alphabetIndex - 1, 0, PLAYER_NAME_CHARS.Length - 1);
                    temp[currentIndex] = PLAYER_NAME_CHARS[alphabetIndex];
                    break;
                case MoveDirection.Left:
                    currentIndex = WrapInt(currentIndex - 1, 0, maxNameLength - 1);
                    alphabetIndex = PLAYER_NAME_CHARS.IndexOf(temp[currentIndex]);
                    break;
                case MoveDirection.Right:
                    currentIndex = WrapInt(currentIndex + 1, 0, maxNameLength - 1);
                    alphabetIndex = PLAYER_NAME_CHARS.IndexOf(temp[currentIndex]);
                    break;
                default:
                    break;
            }
            nameField.text = new string(temp);
            StartCoroutine("Blink");
        }
    }

    public void SubmitTrigger()
    {
        ScoreScript.instance.AddPlayer(nameField.text);
        menuScript.GoToState(3);
    }

    public void CancelTrigger()
    {
        menuScript.GoToState(1);
    }

    private int WrapInt(int value, int min, int max)
    {
        if (value < min)
            return max;
        else if (value > max)
            return min;
        else
            return value;
    }

    private bool isValid(char c)
    {
        return (char.IsLetterOrDigit(c) || c == '_' || c == '-');
    }

    private void StopBlink()
    {
        StopCoroutine("Blink");
        char[] temp = nameField.text.ToCharArray();
        temp[currentIndex] = PLAYER_NAME_CHARS[alphabetIndex];
        nameField.text = new string(temp);
    }

    IEnumerator Blink()
    {
        char[] temp;

        while (true)
        {
            temp = nameField.text.ToCharArray();

            if (isOn)
                temp[currentIndex] = PLAYER_NAME_CHARS[alphabetIndex];
            else
                temp[currentIndex] = BOX;

            isOn = !isOn;
            nameField.text = new string(temp);
            yield return new WaitForSecondsRealtime(blinkTime);
        }
    }
}
