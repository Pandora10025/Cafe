using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camera_Slide : MonoBehaviour
{
    public Transform leftPosition;  // The position of the left side of the room
    public Transform rightPosition; // The position of the right side of the room
    public float slideSpeed = 5f;   // The speed at which the camera slides
    private Vector3 targetPosition; // The current target position of the camera
    private int currentSide = 0;    // 0 = left, 1 = right

    private float initialZ; // To store the camera's initial Z position

    void Start()
    {
        // Store the initial Z position of the camera
        initialZ = transform.position.z;

        // Set the initial target position to the left position's X and Y with the camera's original Z
        targetPosition = new Vector3(leftPosition.position.x, leftPosition.position.y, initialZ);
        transform.position = targetPosition;
    }

    void Update()
    {
        // Switch to the left side
        if (Input.GetKeyDown(KeyCode.A) && currentSide != 0)
        {
            currentSide = 0;
            targetPosition = new Vector3(leftPosition.position.x, leftPosition.position.y, initialZ);
        }

        // Switch to the right side
        if (Input.GetKeyDown(KeyCode.D) && currentSide != 1)
        {
            currentSide = 1;
            targetPosition = new Vector3(rightPosition.position.x, rightPosition.position.y, initialZ);
        }

        // Smoothly interpolate the camera position towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * slideSpeed);
    }
}


