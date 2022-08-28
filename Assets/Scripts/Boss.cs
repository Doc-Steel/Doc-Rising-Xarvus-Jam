using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VaudinGames.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] List<Processor> processors;
    [SerializeField] List<LaserSpawner> spawners;
    [SerializeField] CanvasGroupFader victoryScreen;
    [SerializeField] float overclockRate = 10f;
    private float timeSinceLastOverclock = Mathf.Infinity;
    private List<Processor> activeProcessors;
    private bool dead = false;

    private void Awake()
    {
        SubscribeToProcessors();
    }
    private void Start()
    {
        activeProcessors = processors;
        victoryScreen.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void SubscribeToProcessors()
    {
        foreach (Processor processor in processors)
        {
            processor.broken += OnProcessorBreak;
        }
    }

    private void OnDisable()
    {
        foreach (Processor processor in processors)
        {
            processor.broken -= OnProcessorBreak;
        }
    }

    private void Update()
    {
        if (dead) { return; }
        timeSinceLastOverclock += Time.deltaTime;
        if (timeSinceLastOverclock >= overclockRate)
        {
            OverclockRandomProcessor();
            timeSinceLastOverclock = 0;
        }
    }

    private void OnProcessorBreak(Processor brokenProcessor)
    {
        foreach (LaserSpawner spawner in spawners)
        {
            spawner.burstFire = !spawner.burstFire;
        }
        activeProcessors.Remove(brokenProcessor);
        CheckProcessors();
    }

    private void CheckProcessors()
    {
        foreach (Processor processor in processors)
        {
            if (!processor.IsBroken) { return; }
        }
        OnDeath();
    }

    private void OverclockRandomProcessor()
    {
        Processor overclockTarget = activeProcessors[Random.Range(0, activeProcessors.Count - 1)];
        overclockTarget.Overclock();
    }

    private void OnDeath()
    {
        FindObjectOfType<PlayerMovement>().OnVictory();
        foreach(LaserSpawner spawner in spawners)
        {
            spawner.canFire = false;
        }
        dead = true;
        victoryScreen.FadeIn(3f);
    }
}
