using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointLoader : MonoBehaviour
{
    public GameObject player;
    public VRSceneFadeIn fadeScript; // Drag the black quad's script here

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    private IEnumerator FadeAndLoad()
    {
        // Activate the fade object if it's disabled
        fadeScript.gameObject.SetActive(true);

        // Start fading to black
        yield return StartCoroutine(fadeScript.FadeToBlack());

        // Then load next scene
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

