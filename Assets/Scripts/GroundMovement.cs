using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float speed = 1f; // Adjust this value to control the speed of the movement
    public GameObject tilemapA; // The first tilemap instance
    public GameObject tilemapB; // The second tilemap instance
    private Transform ATransform;
    private Transform BTransform;
    private Renderer ARenderer;
    private Renderer BRenderer;
    private Vector3 initialPosition;
    private float rightmostPosition;
    private float defaultSpeed;

    private bool isARight = false;

    private void Start()
    {
        ATransform = tilemapA.GetComponent<Transform>();
        ARenderer = tilemapA.GetComponent<Renderer>();
        defaultSpeed = speed;

        initialPosition = ATransform.position;
        BTransform = tilemapB.GetComponent<Transform>();
        BRenderer = tilemapB.GetComponent<Renderer>();
        // Ao iniciar o jogo, a posição do tilemap B eh alinhada via script para a direita do tilemap A
        rightmostPosition = ATransform.position.x + (ARenderer.bounds.size.x);
        BTransform.position = new Vector3(
            rightmostPosition,
            ATransform.position.y,
            ATransform.position.z
        );
    }

    private void FixedUpdate()
    {
        if (BTransform.position.x <= initialPosition.x && !isARight)
        {
            isARight = true;
            rightmostPosition = BTransform.position.x + (BRenderer.bounds.size.x);
            ATransform.position = new Vector3(
                rightmostPosition,
                ATransform.position.y,
                ATransform.position.z
            );
        }

        if (ATransform.position.x <= initialPosition.x && isARight)
        {
            isARight = false;
            rightmostPosition = ATransform.position.x + (ARenderer.bounds.size.x);
            BTransform.position = new Vector3(
                rightmostPosition,
                BTransform.position.y,
                BTransform.position.z
            );
        }

        float newPositionA = ATransform.position.x - speed * Time.deltaTime;
        ATransform.position = new Vector3(
            newPositionA,
            ATransform.position.y,
            ATransform.position.z
        );

        float newPositionB = BTransform.position.x - speed * Time.deltaTime;
        BTransform.position = new Vector3(
            newPositionB,
            BTransform.position.y,
            BTransform.position.z
        );
    }

    public void setDefaultSpeed()
    {
        speed = defaultSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
