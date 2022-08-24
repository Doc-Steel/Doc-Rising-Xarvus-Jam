using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 200f;
    [SerializeField] float waypointDistance = 3f;
    [SerializeField] bool canFly;
    [SerializeField] float repeatRate = 0.5f;
    [SerializeField] Transform graphics;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Health health;
    

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.died += OnDeath;
    }

    private void OnDisable()
    {
        health.died -= OnDeath;
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, repeatRate);
        
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        
    }

    private void OnPathComplete(Path p)
    {
        if (p.error) { return; }
        path = p;
        currentWaypoint = 0;
    }

    private void FixedUpdate()
    {
        if (path == null) { return; }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        if (canFly)
        {
            rb.AddForce(force);
        }
        else
        {
            rb.AddForce(new Vector2(force.x, 0));
        }
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < waypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= Mathf.Epsilon)
        {
            graphics.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -Mathf.Epsilon)
        {
            graphics.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
