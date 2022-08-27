using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processor : MonoBehaviour
{
    [SerializeField] float overclockDuration = 5f;
    [SerializeField] Sprite brokenSprite;
    [SerializeField] Sprite workingSprite;
    [SerializeField] Sprite overclockSprite;
    private SpriteRenderer sr;
    private Health health;
    private BoxCollider2D col;
    public Action<Processor> broken;
    public bool IsBroken { get; private set; } = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
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
        col.enabled = true;
        sr.sprite = overclockSprite;
        yield return new WaitForSeconds(overclockDuration);
        col.enabled = false;
        if (!IsBroken) { sr.sprite = workingSprite; }
    }
}
