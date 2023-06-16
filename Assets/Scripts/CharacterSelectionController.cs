using UnityEngine;
using TouchPhase = UnityEngine.TouchPhase;
using UnityEngine.SceneManagement;

public class CharacterSelectionController : MonoBehaviour
{
    public int pixelDistToDetect = 20;
    private Animate _animate;
    private Vector2 _startPos;
    private Vector2 _direction;
    private bool _directionChosen;
    private CharacterAbilityController _abilityController;
    private float timer = 0f;

    private string SwipeDirection()
    {
        _directionChosen = false;
        if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
            return _direction.x > 0 ? "right" : "left";

        return _direction.y > 0 ? "up" : "down";
    }

    private void Start()
    {
        _animate = GetComponent<Animate>();
        _abilityController = GetComponent<CharacterAbilityController>();
    }

    private void Update()
    {
        // Check if the current scene's name is equal to the target scene name
        if (SceneManager.GetActiveScene().name == "main_menu")
        {
            timer += Time.deltaTime;
            if (timer >= 4f)
            {
                // Reset the timer
                timer = 0f;

                // Increment the current element
                if (_animate.change_element == 2)
                {
                    _animate.change_element = 0; // Reset the current element to the first one
                }
                else
                {
                    _animate.change_element++;
                }
            }
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x > Screen.width / 2)
                return;
            if (_abilityController.isDashing == true)
                return;
            if (_abilityController.isGliding == true)
                return;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = touch.position;
                    _directionChosen = true;
                    break;
            }
        }

        if (_directionChosen)
        {
            if (Input.touchCount <= 0)
                return;
            _direction = Input.touches[0].position - _startPos;

            if (
                Mathf.Abs(_direction.x) < pixelDistToDetect
                && Mathf.Abs(_direction.y) < pixelDistToDetect
            )
                return;

            _animate.change_element = SwipeDirection() switch
            {
                "up" => 0,
                "down" => 1,
                "left" => 2,
                // "right" => 3,
                _ => _animate.change_element
            };
        }
    }
}
