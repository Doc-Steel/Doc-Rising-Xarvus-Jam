using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "";
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float fadeWaitTime = 0.5f;
    [SerializeField] Logger logger;
    private SceneManagment sm;

    private void Awake()
    {
        sm = FindObjectOfType<SceneManagment>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(sm.LoadNextLevel(nextSceneName));
        }
    }
}
