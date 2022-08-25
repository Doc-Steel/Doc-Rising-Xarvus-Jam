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
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointDwelltime = 3f;
    private int currentPatrolPointIndex = 0;
    private float timeSinceLastWaypointReached = Mathf.Infinity;

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

    private void Start()
    {
        if (canFly)
        {
            rb.gravityScale = 0;
        }
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
            if (InAttackRangeOfPlayer())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
            else if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceLastWaypointReached = 0;
                    CycleWaypoint();
                }
                if (timeSinceLastWaypointReached > waypointDwelltime)
                {
                    seeker.StartPath(rb.position, GetCurrentWaypoint(), OnPathComplete);
                }
                
            }
            
        }
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentPatrolPointIndex);
    }

    private void CycleWaypoint()
    {
        currentPatrolPointIndex = patrolPath.GetNextIndex(currentPatrolPointIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointDistance;
    }

    private void Update()
    {
        timeSincePathUpdate += Time.deltaTime;
        timeSinceLastWaypointReached += Time.deltaTime;
        laser.canFire = InAttackRangeOfPlayer();
        if (timeSincePathUpdate >= repeatRate)
        {
            UpdatePath();
            timeSincePathUpdate = 0;
        }
    }

    private bool InAttackRangeOfPlayer() => Vector2.Distance(rb.position, target.position) <= attackRange;
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
        if (reachedEndOfPath) { return; }
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
        if (rb.velocity.x >= 0.1f)
        {
            graphics.localScale = new Vector3(-1, graphics.localScale.y, graphics.localScale.z);
        }
        else if (rb.velocity.x <= -0.1f)
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
