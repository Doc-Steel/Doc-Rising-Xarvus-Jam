using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Transform holder;
    [SerializeField] float spawnRate = 1f;
    private float timeSinceLastFire = 0;
    private Transform player;
    public bool canFire = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (!canFire) { return; }
        holder.right = player.position - transform.position;
        transform.right = player.position - transform.position;
        timeSinceLastFire += Time.deltaTime;
        if (timeSinceLastFire > spawnRate)
        {
            Instantiate(laserPrefab, transform.position, transform.rotation);
            timeSinceLastFire = 0;
        }
    }
}
