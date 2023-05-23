using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerController : MonoBehaviour
{
    public int pixelDistToDetect = 20;
    private Animate animate;
    private Vector2 startPos;
    private bool fingerDown;

    void Start()
    {
        animate = GetComponent<Animate>();
    }

    void Update()
    {

        if(fingerDown == false && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began){
                startPos = Input.touches[0].position;
                fingerDown = true;
            }
            if(touch.phase == TouchPhase.Ended)
                fingerDown = false;
        }
        if (fingerDown)
        {
            //Did we swipe up?
            if(Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
            {
                fingerDown = false;
                Debug.Log("Swipe up");
                animate.change_element = 0;
            }
            //Did we swipe down?
            if(Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                fingerDown = false;
                Debug.Log("Swipe down");
                animate.change_element = 1;
            }
            //Did we swipe left?
            else if(Input.touches[0].position.x <= startPos.x - pixelDistToDetect)
            {
                fingerDown = false;
                Debug.Log("Swipe left");
                animate.change_element= 2;
            }
            //Did we swipe right?
            else if(Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
            {
                fingerDown = false;
                Debug.Log("Swipe right");
                animate.change_element= 3;
            }
        }
    }
}