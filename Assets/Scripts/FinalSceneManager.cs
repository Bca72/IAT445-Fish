using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSceneManager : MonoBehaviour
{
    public VRSceneFadeToWhite fadeScript;
    public List<AudioSource> audioSourcesToFade = new List<AudioSource>();
    public float fadeDelay = 45f;
    public float fadeDuration = 2f;

    private void Start()
    {
        StartCoroutine(EndSceneSequence());
    }

    private IEnumerator EndSceneSequence()
    {
        yield return new WaitForSeconds(fadeDelay);

        if (fadeScript != null)
        {
            StartCoroutine(FadeOutAllAudio());
            yield return StartCoroutine(fadeScript.FadeToWhite());
        }

        SceneManager.LoadScene(0);
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

