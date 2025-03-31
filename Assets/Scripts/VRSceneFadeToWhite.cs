using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRSceneFadeToWhite : MonoBehaviour
{
    public float fadeDuration = 3f;
    private Renderer rend;
    private Color targetColor = Color.white;

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
}

