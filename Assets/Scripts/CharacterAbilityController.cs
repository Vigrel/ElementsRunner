using System;
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
    private bool FirstDeath = false;
    public bool AdSeen = false;
    private Animate _animate;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    public GameObject CameraObject;
    private Camera _cam;
    
    // Score
    public GameObject score;
    private ScoreController _scoreScript;

    // WindAbility variables
    public ParticleSystem dashParticleSystem;
    public GameObject obstacleManager;
    public GameObject deathReference;
    public GameObject deathCanva;

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
    public ParticleSystem glideParticleSystem;

    // Start is called before the first frame update
    private void Start()
    {
        dashParticleSystem.Stop();
        glideParticleSystem.Stop();
        _animate = GetComponent<Animate>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _groundMovement = grounds.GetComponent<GroundMovement>();
        _obstacleMovement = obstacleManager.GetComponent<ObstacleMovement>();
        _scoreScript = score.GetComponent<ScoreController>();
        _cam = CameraObject.GetComponent<Camera>();
        deathCanva.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 raycastOrigin = new Vector2(transform.position.x + 0.5f, transform.position.y);
        //Debug.DrawRay(raycastOrigin, Vector2.right * 0.5f, Color.red);
        //
        //Vector2 raycastOrigin2 = new Vector2(transform.position.x - 1.75f, transform.position.y + 0.75f);
        //Debug.DrawRay(raycastOrigin2, Vector2.right * 3.5f, Color.red);
        //
        //Vector2 raycastOrigin3 = new Vector2(transform.position.x, transform.position.y - 0.7f);
        //Debug.DrawRay(raycastOrigin3, Vector2.down*0.25f, Color.blue);

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

        //if (transform.position.x < deathReference.transform.position.x){
        //Debug.Log($"X: {transform.position.x}, ScreenWidth: {Screen.width}");

        Vector3 leftLimit = _cam.ScreenToWorldPoint(new Vector3(-Screen.width / 2, 0, 0));
        if (transform.position.x < leftLimit.x / 2)
        {
            if (FirstDeath == false)
            {
                //Debug.Log("First Death!");
                deathCanva.SetActive(true);
                _scoreScript.PauseScore();
                FirstDeath = true;
            }
            else if (AdSeen == true)
            {
                //Debug.Log("Second Death!");
                _scoreScript.PauseScore();
                _scoreScript.SaveScore();
                SceneManager.LoadScene("end_game");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        //Debug.Log("CollisonEnter");
        if (hit.gameObject.CompareTag("Ground"))
            _isGrounded = true;

        if (hit.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Is Obstacle");
            //// verify if collision was on top of the obstacle
            //Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - 0.85f);
            //RaycastHit2D hit2D = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.25f);
            //Debug.Log(hit2D.collider.tag);

            //if (hit2D)
            //{
            //    Debug.Log(hit2D.collider.tag);
            //    _isGrounded = true;
            //    isJumping = false;
            //}
            List<ContactPoint2D> contacts = new List<ContactPoint2D>();
            hit.GetContacts(contacts);
            //Debug.Log(contacts.Count);
            foreach (ContactPoint2D contact in contacts)
            {
                if (contact.point.y < transform.position.y)
                {
                    //Debug.Log("Is ground");
                    _isGrounded = true;
                    isJumping = false;
                    break;
                }
            }
        }

        //if (hit.gameObject.CompareTag("Obstacle"))
        //    SceneManager.LoadScene("end_game");
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    List<ContactPoint2D> contacts = new List<ContactPoint2D>();
    //    collision.GetContacts(contacts);
    //    foreach (ContactPoint2D contact in contacts)
    //    {
    //        if (contact.point.y < transform.position.y &&
    //            (contact.point.y - transform.position.y) < 0.75f)
    //        {
    //            Debug.Log("Is ground");
    //            _isGrounded = true;
    //            isJumping = false;
    //            break;
    //        }
    //    }
    //}

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
            //float forceY = Mathf.Max(
            //    0f,
            //    (maxVerticalVelocity - currentVelocity.y) * _rb.mass * maxVerticalVelocity / Time.fixedDeltaTime
            //);
            float forceY = _rb.mass * maxVerticalVelocity / Time.fixedDeltaTime;

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
                    if (!_dashStarted)
                        return;
                    _endTime = Time.time;
                    isDashing = true;
                    _dashStarted = false;
                    _dashDuration = Mathf.Max((_endTime - _startTime), 0.2f) * dashMultiplier;

                    // Ignore obstacles
                    _obstacleMovement.IgnoreObstacleCollision(true);

                    // Stop the particle system
                    dashParticleSystem.Stop();

                    // Add transparency to the sprite
                    Color color = _spriteRenderer.color;
                    color.a = dashTransparency;
                    _spriteRenderer.color = color;
                    break;
            }
        }
        if (_dashCooldownTimer > Time.time)
        {
            float cooldownPct = 1f - ((_dashCooldownTimer - Time.time) / dashCooldown);
            Color color = _spriteRenderer.color;
            color.b = cooldownPct;
            color.g = cooldownPct;
            _spriteRenderer.color = color;
        }

        if (isDashing)
        {
            if ((_endTime + _dashDuration) < Time.time)
            {
                if (WillCollide())
                    return;
                _groundMovement.SetDefaultSpeed();
                isDashing = false;
                _dashCooldownTimer = Time.time + dashCooldown;

                // Stop ignoring obstacles
                _obstacleMovement.IgnoreObstacleCollision(false);

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
                    glideParticleSystem.Play();
                    break;

                case TouchPhase.Ended:
                    isGliding = false;
                    _rb.gravityScale = defaultGravityScale;
                    glideParticleSystem.Stop();
                    break;
            }
        }

        if (_isGrounded)
        {
            glideParticleSystem.Stop();
            isGliding = false;
            _rb.gravityScale = defaultGravityScale;
        }
    }

    private bool WillCollide()
    {
        // Perform raycast to the right
        //Vector2 rightOrigin = new Vector2(transform.position.x + 0.5f, transform.position.y);
        //RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, Vector2.right, 0.5f);
        //
        //// Perform raycast to the left
        //Vector2 leftOrigin = new Vector2(transform.position.x - 0.5f, transform.position.y);
        //RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, Vector2.left, 0.5f);
        //
        //if (!hitRight && !hitLeft) return false;
        //return hitRight.collider.CompareTag("Obstacle") || hitRight.collider.CompareTag("Obstacle");

        Vector2 raycastOrigin = new Vector2(
            transform.position.x - 1.75f,
            transform.position.y + 0.75f
        );
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.right, 3.5f);
        if (!hit)
            return false;
        return hit.collider.CompareTag("Obstacle");
    }
}
