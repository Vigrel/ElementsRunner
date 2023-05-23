using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    Animator animator;

    public int change_element;
    public float mov_action;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();    
    }

    void Update()
    {
        animator.SetInteger("ChangeElement", change_element);
        animator.SetFloat("MovementAction", mov_action);
    }
}
