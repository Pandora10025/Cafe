using System.Collections;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject;
    public GameObject cameraFocusPoint;  // The point to focus the camera on
    public SpriteRenderer pitcherSpriteRenderer;
    public Sprite milkSprite;
    public Sprite gmilkSprite;  // Highlighted version of the milk sprite
    public Sprite omilkSprite;  // Original version of the milk sprite

    public Camera mainCamera;  // The main camera
    public float zoomInSize = 3f;  // Target size for zoom in
    public float zoomSpeed;  // Speed of the zoom transition

    private bool isZoomedIn = false;  // Whether the camera is zoomed in
    private float originalCameraSize;
    private Vector3 originalCameraPosition;
    private Coroutine currentCoroutine;  // To track current running coroutine

    // Movement and rotation variables
    public Vector3 targetMilkPosition;  // The target position to move to during zoom in
    private Vector3 originalMilkPosition;  // The original position of the milk object
    public float rotateBackAngle = 9f;  // Final tilt angle after releasing 'S'
    public float maxTiltAngle = 30f;  // Maximum tilt angle when holding 'S'
    public float tiltSpeed = 1f;  // Speed for tilting

    private bool isTilting = false;  // Whether the player is tilting the object
    private float currentAngle = 0f;  // The current tilt angle

    void Start()
    {
        originalCameraSize = mainCamera.orthographicSize;
        originalCameraPosition = mainCamera.transform.position;
        originalMilkPosition = milkObject.transform.position;  // Store the original position of the milk object
    }

    void Update()
    {
        // Check if zoomed in and camera size matches zoomInSize
        if (isZoomedIn && Mathf.Abs(mainCamera.orthographicSize - zoomInSize) < 0.01f)
        {
            // Move the milk object to the target position
            milkObject.transform.position = Vector3.Lerp(milkObject.transform.position, targetMilkPosition, Time.deltaTime * zoomSpeed);

            // Handle tilting logic when pressing 'S'
            if (Input.GetKey(KeyCode.S))
            {
                isTilting = true;
                currentAngle = Mathf.Lerp(currentAngle, maxTiltAngle, Time.deltaTime * tiltSpeed);
            }
            else
            {
                isTilting = false;
                currentAngle = Mathf.Lerp(currentAngle, rotateBackAngle, Time.deltaTime * tiltSpeed);
            }

            // Apply the rotation to the milk object
            milkObject.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        // Mouse on milkObject or not
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit && hit.collider != null && hit.collider.gameObject == milkObject)
        {
            // Change milk object sprite to gmilk sprite when mouse is hovering
            SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
            if (milkSpriteRenderer != null)
            {
                milkSpriteRenderer.sprite = gmilkSprite;
            }

            // Zoom in and change to omilk sprite on left click
            if (Input.GetMouseButtonDown(0) && !isZoomedIn)
            {
                if (milkSpriteRenderer != null)
                {
                    milkSpriteRenderer.sprite = omilkSprite;
                    Debug.Log("Change sprite to omilkSprite: " + milkSpriteRenderer.sprite.name);
                }

                // Start zoom in
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(ZoomInCamera());
            }
        }
        else
        {
            // Change back to original sprite if not zoomed in and sprite is not already omilkSprite
            SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
            if (!isZoomedIn && milkSpriteRenderer != null && milkSpriteRenderer.sprite != omilkSprite)
            {
                milkSpriteRenderer.sprite = milkSprite;
                Debug.Log("Reverting sprite to milkSprite: " + milkSpriteRenderer.sprite.name);
            }
        }

        // Right click to zoom out and reset milk position if tilt is less than 15 degrees
        if (Input.GetMouseButtonDown(1) && isZoomedIn)
        {
            if (Mathf.Abs(currentAngle) < 15f)
            {
                // Reset milk position and rotation if tilt is less than 15 degrees
                ResetMilkPositionAndRotation();
            }

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ZoomOutCamera());
        }
    }

    private IEnumerator ZoomInCamera()
    {
        isZoomedIn = true;
        Vector3 targetPosition = new Vector3(cameraFocusPoint.transform.position.x, cameraFocusPoint.transform.position.y, mainCamera.transform.position.z);

        // Stop using camera slide when zoom in
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = false;
        }

        // Zoom in camera
        while (Mathf.Abs(mainCamera.orthographicSize - zoomInSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomInSize, Time.deltaTime * zoomSpeed);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        mainCamera.orthographicSize = zoomInSize;
        mainCamera.transform.position = targetPosition;
    }

    private IEnumerator ZoomOutCamera()
    {
        isZoomedIn = false;

        // Move back camera
        while (Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalCameraSize, Time.deltaTime * zoomSpeed);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        // Back to original
        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;

        // Camera slide re-activate
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = true;
        }
    }

    private void ResetMilkPositionAndRotation()
    {
        // Reset milk object to original position and rotation
        milkObject.transform.position = originalMilkPosition;  // Move back to original position
        milkObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentAngle = 0f;  // Reset current angle

        // Set sprite back to original
        SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
        if (milkSpriteRenderer != null)
        {
            milkSpriteRenderer.sprite = milkSprite;
        }
    }
}
