using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCreditsTransition : MonoBehaviour
{

    public string Credits;

    private void Start()
    {
        SceneTransitions.Fadein();
        MusicManager.SetMusic("endcredits");
    }

    public void Play()
    {
        GetComponent<Animation>();
        StartCoroutine(LoadAfterAnim());
    }

    public IEnumerator LoadAfterAnim()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}



