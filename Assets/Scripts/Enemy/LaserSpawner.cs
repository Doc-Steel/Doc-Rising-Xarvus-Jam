using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] bool tracksPlayer = true;
    [SerializeField] Transform holder;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] bool burstFire = false;
    private float timeSinceLastFire = 0;
    private Transform player;
    public bool canFire = false;
    private bool firing = false;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (!canFire) { return; }
        if (tracksPlayer)
        {
            SetDirection();
        }
        
        timeSinceLastFire += Time.deltaTime;
        if (timeSinceLastFire > spawnRate && !firing)
        {
            if (burstFire)
            {
                StartCoroutine(FireBurst());
            }
            else
            {
                Fire();
            }

        }
    }

    private void SetDirection()
    {
        holder.right = player.position - transform.position;
        transform.right = player.position - transform.position;
    }

    private void Fire()
    {
        Instantiate(laserPrefab, transform.position, transform.rotation);
        timeSinceLastFire = 0;
    }

    private IEnumerator FireBurst()
    {
        firing = true;
        for (int i = 0; i < 3; i++)
        {
            Instantiate(laserPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
        }
        timeSinceLastFire = 0;
        firing = false;
    }
}
