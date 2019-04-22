using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour {

    private bool isCausingDamage;
    private ParticleSystem particles;
    private List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    private float particleTimer = 0f;

    public float RepeatRate = 0.1f;
    public int DamageAmount = 1;
    public bool Repeating = true;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isCausingDamage = true;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            if (Repeating)
            {
                //Continous Damage
                StartCoroutine(TakeDamage(player, RepeatRate));
            }
            else
            {
                //One Instance of Damage
                player.TakeDamage(DamageAmount);
            }
        }
    }

    private void OnParticleTrigger()
    {
        int numInside = particles.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        if(numInside > 0)
        {
            particleTimer += Time.deltaTime;
            if(particleTimer >= RepeatRate)
            {
                particleTimer = 0f;
                PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                player.TakeDamage(DamageAmount);
            }
        }
    }

    IEnumerator TakeDamage(PlayerController player, float repeatRate)
    {
        while (isCausingDamage)
        {
            player.TakeDamage(DamageAmount);
            TakeDamage(player, repeatRate);
            yield return new WaitForSeconds(repeatRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        
        if (player != null)
        {
            isCausingDamage = false;
        }
    }
}
