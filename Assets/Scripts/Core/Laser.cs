using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] float damage = 10;

    [Header("Audio")]
    [SerializeField] AudioClip laserDeflectSound;
    [SerializeField] AudioClip laserHitSound;

    [Header("Physics")]
    [SerializeField] LayerMask laserLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask playerLayer;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private float timeAlive = 0;
    private bool laserDestroyed = false;
    private bool deflected = false;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Lasers"), LayerMask.NameToLayer("Enemy"), true);
        GetComponents();
    }

    private void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
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
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            if (collision.gameObject.TryGetComponent<Sword>(out Sword sword)) { return; }
            health.TakeDamage(damage);
            DestroyLaser();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Mirror"))
        {
            Sword sword = collision.gameObject.GetComponent<Sword>();
            if (sword.inParryMode)
            {
                Deflect(collision, sword);
            }
            else
            {
                sword.GetComponent<Health>().TakeDamage(damage);
                DestroyLaser();
            }
        }
        else
        {
            DestroyLaser();
        }
    }

    private void Deflect(Collision2D collision, Sword sword)
    {
        sword.ResetParryCooldown();
        audioSource.PlayOneShot(laserDeflectSound);
        ReflectLaserVelocity(collision);
        UpdateLaserRotation();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Lasers"), LayerMask.NameToLayer("Player"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Lasers"), LayerMask.NameToLayer("Enemy"), false);
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
