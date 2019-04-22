using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private AudioSource mAudioSource;
    public AudioClip dialogue;
    private bool playing;

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        playing = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playing == false)
        {
            mAudioSource.PlayOneShot(dialogue);
            playing = true;
        }
    }
}