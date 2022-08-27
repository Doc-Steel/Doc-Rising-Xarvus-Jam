using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Trigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void Bool(string name, bool value)
    {
        animator.SetBool(name, value);
    }
}
