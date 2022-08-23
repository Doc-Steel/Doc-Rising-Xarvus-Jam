using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] float updateSpeed = 0.1f;
    [SerializeField] Health health;

    private void OnEnable()
    {
        health.changed += SetValue;
    }

    private void OnDisable()
    {
        health.changed -= SetValue;
    }

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        SetMaxValue(health.BaseHealth);
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public virtual void SetValue()
    {
        StartCoroutine(TransitionToValue(health.CurrentHealth));
    }

    private IEnumerator TransitionToValue(float value)
    {
        float currentSliderValue = slider.value;
        float elapsed = 0f;
        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(currentSliderValue, value, elapsed / updateSpeed);
            yield return null;
        }
        slider.value = value;
    }
}
