using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointAudioFadeMulti : MonoBehaviour
{
    public List<AudioSource> audioSources = new List<AudioSource>();
    public GameObject player;
    public float fadeDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(FadeOutAllAudio());
        }
    }

    private IEnumerator FadeOutAllAudio()
    {
        float t = 0f;
        float[] startVolumes = new float[audioSources.Count];

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i] != null)
                startVolumes[i] = audioSources[i].volume;
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float factor = 1f - Mathf.Clamp01(t / fadeDuration);

            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i] != null)
                    audioSources[i].volume = startVolumes[i] * factor;
            }

            yield return null;
        }

        foreach (var source in audioSources)
        {
            if (source != null)
                source.volume = 0f;
        }
    }
}

