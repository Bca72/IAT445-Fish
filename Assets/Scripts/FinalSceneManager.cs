using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSceneManager : MonoBehaviour
{
    public VRSceneFadeToWhite fadeScript;
    public float fadeDelay = 45f;

    private void Start()
    {
        StartCoroutine(EndSceneSequence());
    }

    private IEnumerator EndSceneSequence()
    {
        yield return new WaitForSeconds(fadeDelay);

        if (fadeScript != null)
        {
            yield return StartCoroutine(fadeScript.FadeToWhite());
        }

        SceneManager.LoadScene(0);
    }
}


