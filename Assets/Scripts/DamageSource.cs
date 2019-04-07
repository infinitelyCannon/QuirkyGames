using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour {

    private bool isCausingDamage;

    public float RepeatRate = 0.1f;
    public int DamageAmount = 1;
    public bool Repeating = true;

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
