using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    Animator animator;

    public float change_element;
    public float mov_action;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();    
    }

    void Update()
    {
        animator.SetFloat("ChangeElement", change_element);
        animator.SetFloat("MovementAction", mov_action);
        Debug.Log(mov_action);
    }
}
