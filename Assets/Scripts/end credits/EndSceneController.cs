using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    void Start()
    {
        // Start the coroutine to load the main menu after 5 seconds
        StartCoroutine(LoadMainMenuAfterDelay(5f));
    }

    IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the main menu scene
        SceneManager.LoadScene("Main_Menu");
    }
}
