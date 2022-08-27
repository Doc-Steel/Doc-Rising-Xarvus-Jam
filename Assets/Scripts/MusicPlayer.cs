using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] float musicVolumeScale = 0.1f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
        if (audioSource.clip != null)
        {
            StartCoroutine(FadeInMusic(fadeDuration));
        }
    }

    public IEnumerator PlayMusic(AudioClip music)
    {
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOutMusic(fadeDuration));
            audioSource.Stop();
        }
        audioSource.PlayOneShot(music, musicVolumeScale);

        yield return StartCoroutine(FadeInMusic(fadeDuration));
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic(fadeDuration));
    }

    public IEnumerator FadeOutMusic(float fadeDuration)
    {

        while (audioSource.volume > Mathf.Epsilon)
        {
            audioSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator FadeInMusic(float fadeDuration)
    {

        while (audioSource.volume < musicVolumeScale)
        {
            audioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    public IEnumerator WaitForMusicToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
