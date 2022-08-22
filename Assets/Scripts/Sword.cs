using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] float parryWindow = 0.325f;
    [SerializeField] float parryCooldown = 0.8f;
    [SerializeField] AudioClip swordParrySound;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private float timeUntilCanParry = 0;
    public bool inParryMode { get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        sr.color = Color.white;
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

}
