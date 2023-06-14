using UnityEngine;
using TouchPhase = UnityEngine.TouchPhase;


public class CharacterSelectionController : MonoBehaviour
{
    public int pixelDistToDetect = 20;
    private Animate _animate;
    private Vector2 _startPos;
    private Vector2 _direction;
    private bool _directionChosen;

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
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = touch.position;
                    _directionChosen = true;
                    break;
                
                //case TouchPhase.Ended:
                //    _directionChosen = false;
                //    break;
            }
        }

        if (_directionChosen)
        {
            _direction = Input.touches[0].position - _startPos;
            
            if (Mathf.Abs(_direction.x) < pixelDistToDetect && Mathf.Abs(_direction.y) < pixelDistToDetect)
                return;
            
            _animate.change_element = SwipeDirection() switch
            {
                "up" => 0,
                "down" => 1,
                "left" => 2,
                "right" => 3,
                _ => _animate.change_element
            };
        }
    }
    
}
