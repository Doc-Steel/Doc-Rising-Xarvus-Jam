using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] AudioClip laserDeflectSound;
    [SerializeField] AudioClip laserHitSound;
    private SpriteRenderer sr;
    private float timeAlive = 0;
    private AudioSource audioSource;
    private bool laserDestroyed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyLaser();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mirror"))
        {
            Sword sword = collision.gameObject.GetComponent<Sword>();
            if (sword.inParryMode)
            {
                audioSource.PlayOneShot(laserDeflectSound);
                ReflectLaserVelocity(collision);
                UpdateLaserRotation();
            }
            else
            {
                DestroyLaser();
            }
        }
        else
        {
            DestroyLaser();
        }
    }

    private void UpdateLaserRotation()
    {
        var headingChange = Quaternion.FromToRotation(transform.right, rb.velocity.normalized);
        transform.localRotation *= headingChange;
    }

    private void ReflectLaserVelocity(Collision2D collision)
    {
        var speed = rb.velocity.magnitude;
        Vector2 reflectedDirection = Vector2.Reflect(rb.velocity.normalized, collision.GetContact(0).normal);
        rb.velocity = reflectedDirection * speed;
    }

    private void DestroyLaser()
    {
        if (!laserDestroyed)
        {
            StartCoroutine(DestroyLaserRoutine());
            laserDestroyed = true;
        }
    }

    private IEnumerator DestroyLaserRoutine()
    {
        sr.enabled = false;
        rb.simulated = false;
        audioSource.PlayOneShot(laserHitSound);
        float timer = 0;
        while (timer < laserHitSound.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
