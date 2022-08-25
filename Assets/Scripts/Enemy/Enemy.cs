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
    [SerializeField] float attackRange = 10f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Health health;
    private LaserSpawner laser;
    private float timeSincePathUpdate = Mathf.Infinity;
    

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        laser = GetComponentInChildren<LaserSpawner>();
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

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void Update()
    {
        
        timeSincePathUpdate += Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(rb.position, target.position);
        laser.canFire = distanceToPlayer <= attackRange;
        if (distanceToPlayer <= attackRange && timeSincePathUpdate >= repeatRate)
        {
            UpdatePath();
            timeSincePathUpdate = 0;
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
        reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;
        if (reachedEndOfPath) { OnPathComplete(path); }
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

        SetFacing(force);
    }

    private void SetFacing(Vector2 force)
    {
        if (force.x >= Mathf.Epsilon)
        {
            graphics.localScale = new Vector3(-1, graphics.localScale.y, graphics.localScale.z);
        }
        else if (force.x <= -Mathf.Epsilon)
        {
            graphics.localScale = new Vector3(1, graphics.localScale.y, graphics.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
