using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float parryWindow = 0.325f;
    [SerializeField] float parryCooldown = 0.8f;
    [SerializeField] float energyRegenDelay = 2f;

    [Header("Audio")]
    [SerializeField] AudioClip swordParrySound;
    [SerializeField] AudioClip swordBreakSound;
    
    private Health health;
    private BoxCollider2D col;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private float timeUntilCanParry = 0;
    public bool inParryMode { get; private set; }
    public bool isBroken { get; private set; }

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
        GetComponents();
    }

    private void GetComponents()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        sr.color = Color.white;
        isBroken = false;
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
        
        if (Input.GetMouseButtonDown(1) && !inParryMode && !isBroken && timeUntilCanParry <= 0)
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
        audioSource.PlayOneShot(swordBreakSound);
        isBroken = true;
        sr.enabled = false;
        col.enabled = false;
        while (timer <= energyRegenDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        sr.enabled = true;
        col.enabled = true;
        isBroken = false;
        health.Regenerate();
    }

}
