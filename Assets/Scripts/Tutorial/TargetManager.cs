using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    private static int numHit = 0;
    private static TargetManager instance = null;

    private bool done = false;
    private AudioSource audioSource;
    private PlayerController player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (numHit >= 5 && !done && instance == this)
        {
            done = true;
            audioSource.Play();
            StartCoroutine(NextPart());
        }
	}

    IEnumerator NextPart()
    {
        yield return new WaitForSeconds(5f);
        player.deathScreen.FadeIn(2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            numHit++;
        }
    }
}
