using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processor : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float overclockDuration = 5f;

    [Header("Sprites")]
    [SerializeField] Sprite brokenSprite;
    [SerializeField] Sprite workingSprite;
    [SerializeField] Sprite overclockSprite;

    [Header("Sounds")]
    [SerializeField] AudioClip breakSound;
    [SerializeField] AudioClip overclockSound;
    private SpriteRenderer sr;
    private Health health;
    private BoxCollider2D col;
    public Action<Processor> broken;
    private AudioSource audioSource;
    public bool IsBroken { get; private set; } = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider2D>();
        health.died += Break;
    }

    private void Start()
    {
        col.enabled = false;
        sr.sprite = workingSprite;
    }

    private void OnDisable()
    {
        health.died -= Break;
    }

    private void Break()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(breakSound);
        IsBroken = true;
        sr.sprite = brokenSprite;
        broken.Invoke(this);
    }

    public void Overclock()
    {
        StartCoroutine(OverclockRoutine());
    }

    private IEnumerator OverclockRoutine()
    {
        audioSource.PlayOneShot(overclockSound);
        col.enabled = true;
        sr.sprite = overclockSprite;
        yield return new WaitForSeconds(overclockDuration);
        col.enabled = false;
        if (!IsBroken) { sr.sprite = workingSprite; }
    }
}
