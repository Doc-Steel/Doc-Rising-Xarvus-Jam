using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float BaseHealth { get; private set; } = 100;
    [SerializeField] public float iSeconds = 0.1f;
    public Action died;
    public Action changed;
    public bool IsDead { get; private set; }

    private float timeSinceDamaged = Mathf.Infinity;
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = BaseHealth;
    }

    private void Update()
    {
        timeSinceDamaged += Time.deltaTime;
    }

    public void GainHealth(float value)
    {
        CurrentHealth += value;
        changed?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || timeSinceDamaged < iSeconds) { return; }
        CurrentHealth -= damage;

        StartCoroutine(FlashCoroutine());

        if (CurrentHealth <= 0)
        {
            died?.Invoke();
            IsDead = true;
        }
        changed?.Invoke();

        timeSinceDamaged = 0;
    }

    public void Regenerate()
    {
        CurrentHealth = BaseHealth;
        IsDead = false;
        changed?.Invoke();
    }

    private IEnumerator FlashCoroutine()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        List<Color> baseColors = new List<Color>();
        float duration = iSeconds;
        while (duration > 0)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                baseColors.Add(spriteRenderer.color);
                spriteRenderer.color = Color.red;
            }
            duration -= Time.deltaTime;


            yield return null;
        }
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = baseColors[i];
        }
    }
}
