using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterAbilityController : MonoBehaviour
{
    public float maxVerticalVelocity = 7f;
    private bool _touchedScreen;
    private bool _isGrounded = true;
    private Animate _animate;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    private void Start()
    {
        _animate = GetComponent<Animate>();
        _rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        switch (_animate.change_element)
        {
            case 0:
                FireAbility();
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void FireAbility()
    {
        if (Input.touchCount > 0 && _isGrounded)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < (Screen.width / 2)) return;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchedScreen = true;
                    break;
            }
        }

        if (_touchedScreen && _isGrounded)
        {
            // Calculate the force needed to reach the maximum allowed velocity
            Vector2 currentVelocity = _rb.velocity;
            float forceY = Mathf.Max(0f, (maxVerticalVelocity - currentVelocity.y) * _rb.mass / Time.fixedDeltaTime);
        
            // Clamp the force to the maximum jump force and add it to the Rigidbody
            Vector2 forceToAdd = new Vector2(0f, Mathf.Clamp(forceY, 0f, 400f));
            _rb.AddForce(forceToAdd);

            _touchedScreen = false;
        }
    }
}