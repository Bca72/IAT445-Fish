using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSceneFadeIn : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    private Renderer rend;
    private Color startColor;
    [Range(0f, 1f)]public float fadeInPortion = 0.3f; // Portion of fadeDuration used to fade in

    public AudioSource audioSource;
    public AudioClip clipToPlayAfterFade;


    private void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void Start()
    {
        // Ensure it's fully black at start
        Color color = startColor;
        color.a = 1f;
        rend.material.color = color;

        // Start fading in
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color color = rend.material.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            rend.material.color = color;
            yield return null;
        }

        color.a = 0f;
        rend.material.color = color;

        // 🎵 Play audio after fade is done
        if (audioSource != null && clipToPlayAfterFade != null)
        {
            audioSource.clip = clipToPlayAfterFade;
            audioSource.Play();
        }

        // Optional: disable the object after fade
        gameObject.SetActive(false);
    }


    public IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color color = rend.material.color;
        color.a = 0f;
        rend.material.color = color;

        float fadeInTime = fadeDuration * fadeInPortion;
        float holdTime = fadeDuration - fadeInTime;

        // Step 1: Fade in quickly
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeInTime);
            color.a = Mathf.Lerp(0f, 1f, progress);
            rend.material.color = color;
            yield return null;
        }

        // Step 2: Stay fully black
        color.a = 1f;
        rend.material.color = color;
        yield return new WaitForSeconds(holdTime);
    }


}
