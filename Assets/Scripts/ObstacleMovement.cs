using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Tilemaps;

public class ObstacleComponents 
{
    public GameObject Object { get; }
    public Transform Transform { get; }
    //public TilemapCollider2D Collider { get; }
    public CompositeCollider2D Collider { get; }
    
    //public ObstacleComponents(GameObject _object, Transform _transform, TilemapCollider2D _collider)
    public ObstacleComponents(GameObject _object, Transform _transform, CompositeCollider2D _collider)
    {
        Object = _object;
        Transform = _transform;
        Collider = _collider;
    }
}

public class ObstacleMovement : MonoBehaviour
{
    // Passed variables
    public GameObject grounds;
    public GameObject groundA;
    public GameObject groundB;
    public List<GameObject> obstacles;
    public GameObject player;
    
    // Ground movement variables
    private GroundMovement _groundScript;
    private Transform _aTransform;
    private Transform _bTransform;
    private CharacterAbilityController _playerAbilityScript;
    
    // Obstacle movement variables
    private List<ObstacleComponents> _obstacleComponentsList = new List<ObstacleComponents>();
    
    // Current obstacle variables
    private CapsuleCollider2D _playerCollider;
    private ObstacleComponents _currObstacleA;
    private ObstacleComponents _currObstacleB;
    private bool _localIsARight;

    private ObstacleComponents GetRandomObstacle()
    {
        Random rnd = new Random();
        return _obstacleComponentsList.Where(obst => obst != _currObstacleA && obst != _currObstacleB)
                                      .OrderBy(obst => rnd.Next())
                                      .First();
    }

    public void IgnoreObstacleCollision(bool ignore)
    {
        if (_currObstacleA != null)
            Physics2D.IgnoreCollision(_playerCollider, _currObstacleA.Collider, ignore); 
            //_currObstacleA.Collider.enabled = state;
            
        if(_currObstacleB != null)
            Physics2D.IgnoreCollision(_playerCollider, _currObstacleB.Collider, ignore); 
            //_currObstacleB.Collider.enabled = state;
    }
    
    
    private void FillObstaclesComponentsList()
    {
        foreach (var el in obstacles)
        {
            _obstacleComponentsList.Add(
                new ObstacleComponents(
                    el,
                    el.GetComponent<Transform>(), 
                    //el.GetComponent<TilemapCollider2D>())
                    el.GetComponent<CompositeCollider2D>())
                );
        }
    }

    private void Start()
    {
        FillObstaclesComponentsList();
        
        _playerAbilityScript = player.GetComponent<CharacterAbilityController>();
        _playerCollider = player.GetComponent<CapsuleCollider2D>();
        
        _groundScript = grounds.GetComponent<GroundMovement>();
        _aTransform = groundA.transform;
        _bTransform = groundB.transform;

        _currObstacleB = GetRandomObstacle();
    }

    private void FixedUpdate()
    {
        if (!_localIsARight && _groundScript.isARight)
        {
            _currObstacleA = GetRandomObstacle();
            _localIsARight = true;
        }
        
        if (_localIsARight && !_groundScript.isARight)
        {
            _currObstacleB = GetRandomObstacle();
            _localIsARight = false;
        }

        if (_currObstacleA != null)
            _currObstacleA.Transform.position = _aTransform.position;

        if (_currObstacleB != null)
            _currObstacleB.Transform.position = _bTransform.position;
    }
}
