using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Tilemaps;

public class ObstacleMovement : MonoBehaviour
{
    public GameObject grounds;
    public GameObject groundA;
    public GameObject groundB;
    public List<GameObject> obstacles;
    public GameObject player;
    
    private GroundMovement _groundScript;
    private Transform _aTransform;
    private Transform _bTransform;
    private CharacterAbilityController _playerAbilityScript;
    
    private GameObject _currObstacle;
    private Transform _currTransform;
    private TilemapCollider2D _currCollider;
    private bool _localIsARight;

    GameObject GetRandomObstacle()
    {
        Random rnd = new Random();
        return obstacles.OrderBy(obst => rnd.Next()).First();
    }

    void SetCurrentObstacleTransform()
    {
        _playerAbilityScript = player.GetComponent<CharacterAbilityController>();
        _currTransform = _currObstacle.GetComponent<Transform>();
        _currCollider = _currObstacle.GetComponent<TilemapCollider2D>();
    }

    public void SetCurrentObstacleCollider(bool state)
    {
        _currCollider.enabled = state;
    }

    void Start()
    {
        _groundScript = grounds.GetComponent<GroundMovement>();
        _aTransform = groundA.transform;
        _bTransform = groundB.transform;

        _currObstacle = GetRandomObstacle();
        SetCurrentObstacleTransform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currTransform.position = _bTransform.position;
    }
}
