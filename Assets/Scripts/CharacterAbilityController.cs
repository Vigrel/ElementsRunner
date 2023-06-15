using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

// using System;

public class CharacterAbilityController : MonoBehaviour
{
    // General variables
    //private bool _touchedScreen;
    private bool _isGrounded = true;
    private Animate _animate;
    private Rigidbody2D _rb;

    // WindAbility variables
    public ParticleSystem dashParticleSystem;
    public GameObject obstacleManager;
    private ObstacleMovement _obstacleMovement;
    public float dashMultiplier = 1f;
    public float dashCooldown;
    public float dashTransparency = 0.2f;  
    public GameObject grounds;
    public bool isDashing = false;
    private bool _dashStarted = false;
    private float _startTime;
    private float _endTime;
    private float _dashDuration;
    private GroundMovement _groundMovement;
    private SpriteRenderer _spriteRenderer;
    private float _dashCooldownTimer;

    // FireAbility variables
    public float maxVerticalVelocity = 7f;
    public bool isJumping = false;
    
    // WaterAbility variables
    public float defaultGravityScale = 1f;
    public float glideGravityScale = 0.5f;
    public bool isGliding;

    // Start is called before the first frame update
    private void Start()
    {
        dashParticleSystem.Stop();
        _animate = GetComponent<Animate>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _groundMovement = grounds.GetComponent<GroundMovement>();
        _obstacleMovement = obstacleManager.GetComponent<ObstacleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_animate.change_element)
        {
            case 0:
                FireAbility();
                break;
            case 1:
                WaterAbility();
                break;
            case 2:
                WindAbility();
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
            _isGrounded = true;

        if (hit.gameObject.CompareTag("Obstacle"))
            SceneManager.LoadScene("end_game");
    }

    private void FireAbility()
    {
        if (Input.touchCount > 0 && _isGrounded)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < (Screen.width / 2))
                return;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isJumping = true;
                    break;
            }
        }

        if (isJumping && _isGrounded)
        {
            // Calculate the force needed to reach the maximum allowed velocity
            Vector2 currentVelocity = _rb.velocity;
            float forceY = Mathf.Max(
                0f,
                (maxVerticalVelocity - currentVelocity.y) * _rb.mass * maxVerticalVelocity / Time.fixedDeltaTime
            );

            // Clamp the force to the maximum jump force and add it to the Rigidbody
            Vector2 forceToAdd = new Vector2(0f, forceY);
            _rb.AddForce(forceToAdd);
            _isGrounded = false;

            isJumping = false;
        }
    }

    private void WindAbility()
    {
        if (Input.touchCount > 0 && !isDashing && (_dashCooldownTimer < Time.time))
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < (Screen.width / 2))
                return;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _dashStarted = true;
                    _startTime = Time.time;
                    // Play the particle system
                    dashParticleSystem.Play();
                    break;

                case TouchPhase.Ended:
                    if (!_dashStarted) return;
                    _endTime = Time.time;
                    isDashing = true;
                    _dashStarted = false;
                    _dashCooldownTimer = _dashDuration + dashCooldown;
                    _dashDuration = Mathf.Max((_endTime - _startTime), 0.2f) * dashMultiplier;
                    _obstacleMovement.SetCurrentObstacleCollider(false);
                    
                    // Stop the particle system
                    dashParticleSystem.Stop();
                    
                    // Add transparency to the sprite
                    Color color = _spriteRenderer.color;
                    color.a = dashTransparency;
                    _spriteRenderer.color = color;
                    break;
            }
        }

        if (isDashing)
        {
            if ((_endTime + _dashDuration) < Time.time)
            {
                _groundMovement.SetDefaultSpeed();
                isDashing = false;
                _obstacleMovement.SetCurrentObstacleCollider(true);
                // Remove transparency from the sprite
                Color color = _spriteRenderer.color;
                color.a = 1f;
                _spriteRenderer.color = color;
                return;
            }

            _groundMovement.SetSpeed(10 * Time.deltaTime + _groundMovement.speed);
        }
    }

    private void WaterAbility()
    {
        if (Input.touchCount > 0 && !_isGrounded)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < (Screen.width / 2))
                return;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isGliding = true;
                    _rb.gravityScale = glideGravityScale;
                    break;

                case TouchPhase.Ended:
                    isGliding = false;
                    _rb.gravityScale = defaultGravityScale;
                    break;
            }
        }

        if (_isGrounded)
        {
            isGliding = false;
            _rb.gravityScale = defaultGravityScale;
        }
    }
}