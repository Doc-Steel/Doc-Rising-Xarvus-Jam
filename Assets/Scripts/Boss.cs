using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] List<Processor> processors;
    [SerializeField] float overclockRate = 10f;
    private float timeSinceLastOverclock = Mathf.Infinity;
    private List<Processor> activeProcessors;

    private void Awake()
    {
        SubscribeToProcessors();
    }
    private void Start()
    {
        activeProcessors = processors;
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
        timeSinceLastOverclock += Time.deltaTime;
        if (timeSinceLastOverclock >= overclockRate)
        {
            OverclockRandomProcessor();
            timeSinceLastOverclock = 0;
        }
    }

    private void OnProcessorBreak(Processor brokenProcessor)
    {
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

    }
}
