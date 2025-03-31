using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRingCheckpoint : MonoBehaviour
{
    public string playerTag = "Player";
    public AudioSource audioSource;
    public float destroyDelay = 1.0f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag(playerTag))
        {
            triggered = true;

            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Destroy after audio finishes (or fixed delay)
            Destroy(gameObject, audioSource != null ? audioSource.clip.length : destroyDelay);
        }
    }
}

