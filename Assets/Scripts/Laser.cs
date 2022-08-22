using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 10f;
    private float timeAlive = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mirror"))
        {
            Sword sword = collision.gameObject.GetComponent<Sword>();
            if (sword.inParryMode)
            {
                ReflectLaserVelocity(collision);
                UpdateLaserRotation();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
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
}
