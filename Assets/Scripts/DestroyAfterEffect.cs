using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void Notify();

public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystems = null;
    public static event Notify impactComplete;

    void Update()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem.IsAlive())
            {
                continue;
            }
        }
        if (!GetComponent<AudioSource>().isPlaying)
        {
            impactComplete?.Invoke();
            Destroy(gameObject);
        }
        
    }
}
