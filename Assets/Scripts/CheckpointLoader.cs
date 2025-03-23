using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointLoader : MonoBehaviour
{
    public GameObject player;
    public VRSceneFadeIn fadeScript; // Drag the black quad's script here
    public GameObject audioGroup; // Drag your GameObject with 3 AudioSources
    public float audioFadeDuration = 1.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(FadeAndLoad());
        }
    }
    private IEnumerator FadeOutAudio()
    {
        if (audioGroup == null) yield break;

        AudioSource[] sources = audioGroup.GetComponents<AudioSource>();
        float[] originalVolumes = new float[sources.Length];

        for (int i = 0; i < sources.Length; i++)
            originalVolumes[i] = sources[i].volume;

        float t = 0f;
        while (t < audioFadeDuration)
        {
            t += Time.deltaTime;
            float fadeFactor = 1f - (t / audioFadeDuration);
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != null)
                    sources[i].volume = originalVolumes[i] * fadeFactor;
            }
            yield return null;
        }

        // Ensure they’re fully silent
        foreach (var source in sources)
        {
            if (source != null)
                source.volume = 0f;
        }
    }

    private IEnumerator FadeAndLoad()
    {
        if (fadeScript != null)
            fadeScript.gameObject.SetActive(true);

        // Start both fades at the same time
        Coroutine fadeBlack = StartCoroutine(fadeScript.FadeToBlack());
        Coroutine fadeAudio = StartCoroutine(FadeOutAudio());

        yield return fadeBlack; // Wait for screen fade
        yield return fadeAudio; // Optional: wait for audio fade if different timing

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in build settings!");
        }
    }

}

