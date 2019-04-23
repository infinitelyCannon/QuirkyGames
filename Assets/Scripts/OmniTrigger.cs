using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OmniTrigger : MonoBehaviour
{
    private AudioSource mAudioSource;
    public AudioClip dialogue;
    public float typeDelay;
    public float typeWaitDelay;
    public float lifeTime;
    public int nextSceneIndex;
    [TextArea] public string script;

    //Different functions of the TriggerBox
    public bool playDialogue;
    public bool activateJetPack;
    public bool activateCannon;
    public bool advanceScene;


    private Text dialogText;
    private string textBuffer = "";
    private delegate void AfterEffect();

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playDialogue == true)
        {
            mAudioSource.PlayOneShot(dialogue);
            playDialogue = false;
            StartCoroutine(Type(typeWaitDelay, script, lifeTime, () => { Clear(); }));
        }
        if (other.gameObject.CompareTag("Player") && activateJetPack == true)
        {
            other.gameObject.GetComponent<PlayerController>().ActivateJetPack();
        }
        if (other.gameObject.CompareTag("Player") && activateCannon == true)
        {
            other.gameObject.GetComponentInChildren<OnFireScript>().cannonEquipped = true;
        }
        if (other.gameObject.CompareTag("Player") && advanceScene == true)
        {
            other.gameObject.GetComponent<PlayerController>().deathScreen.FadeIn(nextSceneIndex);

        }
    }

    private void Clear()
    {
        dialogText.text = dialogText.text.Replace(script, "");
    }

    IEnumerator Type(float startDelay, string message, float endDelay, AfterEffect action)
    {
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