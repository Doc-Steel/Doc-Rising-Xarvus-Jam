using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float spawnRate = 1f;
    private float timeSinceLastFire = 0;

    private void Update()
    {
        timeSinceLastFire += Time.deltaTime;
        if (timeSinceLastFire > spawnRate)
        {
            Instantiate(laserPrefab, transform.position, transform.rotation);
            timeSinceLastFire = 0;
        }
    }
}
