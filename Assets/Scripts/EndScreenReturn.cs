using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenReturn: MonoBehaviour
{
    public string restartSceneName = "Kelp Caves"; // Set this to the scene you want to return to

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // If at the last scene, return to the specified scene
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(restartSceneName);
        }
        else
        {
            SceneManager.LoadScene(nextIndex);
        }
    }
}