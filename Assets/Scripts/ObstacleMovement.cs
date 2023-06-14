using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;

public class ObstacleMovement : MonoBehaviour
{
    public GameObject grounds;
    public GameObject groundA;
    public GameObject groundB;
    public List<GameObject> obstacles;

    private GroundMovement _groundScript;
    private Transform _aTransform;
    private Transform _bTransform;
    
    private GameObject _currObstacle;
    private Transform _currTransform;
    private bool _localIsARight;

    GameObject GetRandomObstacle()
    {
        Random rnd = new Random();
        return obstacles.OrderBy(obst => rnd.Next()).First();
    }

    void SetCurrentObstacleTransform()
    {
        _currTransform = _currObstacle.GetComponent<Transform>();
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
    void Update()
    {
        _currTransform.position = _bTransform.position;
    }
}
