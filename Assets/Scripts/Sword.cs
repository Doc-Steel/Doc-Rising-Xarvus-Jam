using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] float parryWindow = 0.325f;
    [SerializeField] float parryRecovery = 0.8f;
    private SpriteRenderer sr;
    private float timeSinceLastParry = Mathf.Infinity;
    public bool inParryMode { get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }

    private void Update()
    {
        if (!inParryMode)
        {
            timeSinceLastParry += Time.deltaTime;
        }
        
        if (Input.GetMouseButtonDown(1) && !inParryMode && timeSinceLastParry >= parryRecovery)
        {
            inParryMode = true;
            StartCoroutine(Parry());
        }
    }

    private IEnumerator Parry()
    {
        sr.color = Color.blue;
        float timer = 0;
        while (timer < parryWindow)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        inParryMode = false;
        sr.color = Color.white;
    }

}
