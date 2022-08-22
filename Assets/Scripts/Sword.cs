using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] float parryWindow = 0.325f;
    [SerializeField] float parryCooldown = 0.8f;
    [SerializeField] AudioClip swordParrySound;
    [SerializeField] float energyRegenDelay = 2f;
    private Health health;
    private BoxCollider2D col;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private float timeUntilCanParry = 0;
    public bool inParryMode { get; private set; }

    private void OnEnable()
    {
        health.died += OnSwordBreak;
    }

    private void OnDisable()
    {
        health.died -= OnSwordBreak;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        col = GetComponent<BoxCollider2D>();
        sr.color = Color.white;
    }

    private void OnSwordBreak()
    {
        StartCoroutine(RegenerateSword());
    }

    private void Update()
    {
        if (!inParryMode)
        {
            timeUntilCanParry -= Time.deltaTime;
        }
        
        if (Input.GetMouseButtonDown(1) && !inParryMode && timeUntilCanParry <= 0)
        {
            inParryMode = true;
            timeUntilCanParry = parryCooldown;
            StartCoroutine(Parry());
        }
    }


    private IEnumerator Parry()
    {
        audioSource.PlayOneShot(swordParrySound);
        sr.color = Color.blue;
        float timer = 0;
        while (timer < parryWindow)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        inParryMode = false;
        sr.color = Color.white;
    }

    public void ResetParryCooldown()
    {
        timeUntilCanParry = 0;
    }

    private IEnumerator RegenerateSword()
    {
        float timer = 0;
        sr.enabled = false;
        col.enabled = false;
        while (timer <= energyRegenDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        sr.enabled = true;
        col.enabled = true;
        health.Regenerate();
    }

}
