using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines the borders of 'Player's' movement. Depending on the chosen handling type, it moves the 'Player' with keyboard inputs (ZQSD).
/// </summary>

[System.Serializable]
public class Borders
{
    [Tooltip("Offset from viewport borders for player's movement")]
    public float minXOffset = 1.5f, maxXOffset = 1.5f, minYOffset = 1.5f, maxYOffset = 1.5f;
    [HideInInspector] public float minX, maxX, minY, maxY;
}

public class PlayerMoving : MonoBehaviour {

    [Tooltip("Offset from viewport borders for player's movement")]
    public Borders borders;
    Camera mainCamera;
    bool controlIsActive = true; 

    public static PlayerMoving instance; // Unique instance of the script for easy access to the script

    public float speed = 20f; // Movement speed for ZQSD

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        ResizeBorders(); // Setting 'Player's' moving borders depending on viewport's size
    }

    private void Update()
    {
        if (controlIsActive)
        {
            HandleKeyboardInput();
            ClampPositionWithinBorders();
        }
    }

    // Handle movement with ZQSD keys
    private void HandleKeyboardInput()
    {
        Vector3 movement = new Vector3();

        if (Input.GetKey(KeyCode.W)) movement.y += 1; // Move up
        if (Input.GetKey(KeyCode.S)) movement.y -= 1; // Move down
        if (Input.GetKey(KeyCode.A)) movement.x -= 1; // Move left
        if (Input.GetKey(KeyCode.D)) movement.x += 1; // Move right

        transform.position += movement.normalized * speed * Time.deltaTime;
    }

    private void ClampPositionWithinBorders()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, borders.minX, borders.maxX),
            Mathf.Clamp(transform.position.y, borders.minY, borders.maxY),
            transform.position.z
        );
    }

    // Setting 'Player's' movement borderss according to viewport size and defined offset
    void ResizeBorders() 
    {

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector3 topRight = mainCamera.ViewportToWorldPoint(Vector2.one);

        borders.minX = bottomLeft.x + borders.minXOffset -10f;
        borders.minY = bottomLeft.y + borders.minYOffset +10f;
        borders.maxX = topRight.x - borders.maxXOffset +10f;
        borders.maxY = topRight.y - borders.maxYOffset +20f;
    }
}