using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

	private Animate animate;

    void Start()
        {
            animate = GetComponent<Animate>();
        }

    void Update(){
        animate.change_element = Input.GetAxisRaw("Horizontal");
        animate.mov_action = Input.GetAxisRaw("Vertical");
        Debug.Log(Input.GetAxisRaw("Vertical"));
    }

}
