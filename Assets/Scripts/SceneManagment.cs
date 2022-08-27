using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VaudinGames.UI;

public class SceneManagment : MonoBehaviour
{
    [SerializeField] CanvasGroupFader cgf;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float levelTransitionTime = 2f;
    [SerializeField] float deathFadeTime = 1f;
    [SerializeField] MusicPlayer mp;

    private void Start()
    {
        cgf.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        cgf.FadeOut(fadeInTime);
    }
    public IEnumerator RestartLevel()
    {
        cgf.FadeIn(deathFadeTime);
        yield return new WaitForSeconds(deathFadeTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    

    public IEnumerator LoadNextLevel(string sceneName)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) < 0)
        {
            Debug.LogError($"Scene name: {sceneName} is not a valid scene.");
        }
        cgf.FadeIn(levelTransitionTime);
        if (mp) {
            StartCoroutine(mp.FadeOutMusic(levelTransitionTime));
        }
        yield return new WaitForSeconds(levelTransitionTime);
        SceneManager.LoadScene(sceneName);

    }    
}
