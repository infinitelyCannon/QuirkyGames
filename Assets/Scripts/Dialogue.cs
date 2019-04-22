using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    private AudioSource mAudioSource;
    public AudioClip dialogue;
    public float typeDelay;
    public float lifeTime;
    [TextArea] public string script;

    private bool playing;
    private Text dialogText;
    private string textBuffer = "";
    private delegate void AfterEffect();

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        playing = false;
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playing == false)
        {
            mAudioSource.PlayOneShot(dialogue);
            playing = true;
            StartCoroutine(Type(0f, script, lifeTime, () => { Clear(); }));
        }
    }

    private void Clear()
    {
        dialogText.text = dialogText.text.Replace(script, "");
    }

    IEnumerator Type(float startDelay, string message, float endDelay, AfterEffect action)
    {
        char[] temp = dialogText.text.ToCharArray();
        int msgIdx = 0, lastIdx = 0;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        while(msgIdx < message.Length)
        {
            dialogText.text = dialogText.text.Insert(lastIdx++, message.Substring(msgIdx++, 1));
            yield return new WaitForSeconds(typeDelay);
        }

        if (endDelay > 0f)
            yield return new WaitForSeconds(endDelay);

        action?.Invoke();
    }
}