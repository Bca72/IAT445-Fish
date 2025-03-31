using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRSceneFadeToWhite : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Renderer rend;
    private Color targetColor = Color.white;

    public List<AudioSource> audioSourcesToFade = new List<AudioSource>();

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        Color initial = targetColor;
        initial.a = 0f;
        rend.material.color = initial;
        gameObject.SetActive(false);
    }

    public IEnumerator FadeToWhite()
    {
        gameObject.SetActive(true);

        float t = 0f;
        Color color = rend.material.color;
        color.a = 0f;
        rend.material.color = color;

        // Start audio fade
        StartCoroutine(FadeOutAllAudio());

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeDuration);
            color.a = Mathf.Lerp(0f, 1f, progress);
            rend.material.color = color;
            yield return null;
        }

        color.a = 1f;
        rend.material.color = color;
    }

    private IEnumerator FadeOutAllAudio()
    {
        float t = 0f;
        float[] startVolumes = new float[audioSourcesToFade.Count];

        for (int i = 0; i < audioSourcesToFade.Count; i++)
        {
            if (audioSourcesToFade[i] != null)
                startVolumes[i] = audioSourcesToFade[i].volume;
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float factor = Mathf.Clamp01(t / fadeDuration);

            for (int i = 0; i < audioSourcesToFade.Count; i++)
            {
                if (audioSourcesToFade[i] != null)
                    audioSourcesToFade[i].volume = Mathf.Lerp(startVolumes[i], 0f, factor);
            }

            yield return null;
        }

        foreach (var source in audioSourcesToFade)
        {
            if (source != null)
                source.volume = 0f;
        }
    }
}

