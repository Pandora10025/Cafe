using System.Collections;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject;
    public GameObject cameraFocusPoint;  // The point to focus the camera on
    public SpriteRenderer pitcherSpriteRenderer;
    public Sprite originalPitcherSprite;
    public Sprite pitcherWithMilk1;
    public Sprite pitcherWithMilk2;
    public Animator milkObjectAnimator;  // Animator for the milk object

    public Camera mainCamera;  // The main camera
    public float zoomInSize = 3f;  // Target size for zoom in
    public float zoomSpeed;  // Speed of the zoom transition

    private bool isZoomedIn = false;  // Whether the camera is zoomed in
    private float originalCameraSize;
    private Vector3 originalCameraPosition;
    private Coroutine currentCoroutine;  // To track current running coroutine

    // Movement variables
    public Vector3 targetMilkPosition;  // The target position to move to during zoom in
    private Vector3 originalMilkPosition;  // The original position of the milk object
    private float sKeyPressedTime = 0f;  // Track how long S key is pressed
    private bool isPouring = false;  // Whether pouring is in progress

    private Draggable draggableScript;  // Reference to the draggable script

    public AudioSource PourMilk;

    void Start()
    {
        originalCameraSize = mainCamera.orthographicSize;
        originalCameraPosition = mainCamera.transform.position;
        originalMilkPosition = milkObject.transform.position;  // Store the original position of the milk object

        // Get the draggable script attached to the milkObject
        draggableScript = milkObject.GetComponent<Draggable>();
    }

    void Update()
    {
        // Handle mouse hover to trigger highlight animation
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == milkObject)
        {
            if (milkObjectAnimator != null)
            {
                milkObjectAnimator.SetTrigger("Highlight");
            }

            // Start zoom in process if clicked and pitcher sprite is original
            if (Input.GetMouseButtonDown(0) && pitcherSpriteRenderer.sprite == originalPitcherSprite && !isZoomedIn)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(ZoomInCamera());
            }
        }
        else
        {
            if (milkObjectAnimator != null)
            {
                milkObjectAnimator.SetTrigger("Idle");
            }
        }

        if (isZoomedIn && Input.GetKey(KeyCode.S))
        {
            sKeyPressedTime += Time.deltaTime;

            // Only play PourMilk sound once when pouring starts
            if (!isPouring)
            {
                PourMilk.Play();
                isPouring = true; // Start pouring process
                if (milkObjectAnimator != null)
                {
                    milkObjectAnimator.SetTrigger("MilkReady");
                }
            }

            // Update pitcher sprite when specific conditions are met
            if (sKeyPressedTime >= 2f && pitcherSpriteRenderer.sprite != pitcherWithMilk1)
            {
                pitcherSpriteRenderer.sprite = pitcherWithMilk1;
            }

            if (sKeyPressedTime >= 4f && pitcherSpriteRenderer.sprite != pitcherWithMilk2)
            {
                pitcherSpriteRenderer.sprite = pitcherWithMilk2;
                currentCoroutine = StartCoroutine(ResetMilkAndZoomOut());
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            // Stop the pouring sound and reset pouring state when the S key is released
            if (isPouring)
            {
                PourMilk.Stop();
                isPouring = false;

                if (milkObjectAnimator != null)
                {
                    milkObjectAnimator.SetTrigger("MilkBack");
                }
            }

            sKeyPressedTime = 0f;
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

        // Move the milk object to the target position smoothly
        while (Vector3.Distance(milkObject.transform.position, targetMilkPosition) > 0.01f)
        {
            milkObject.transform.position = Vector3.Lerp(milkObject.transform.position, targetMilkPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        milkObject.transform.position = targetMilkPosition;
    }

    private IEnumerator ResetMilkAndZoomOut()
    {
        isZoomedIn = false;

        // Move milk object back to original position smoothly
        while (Vector3.Distance(milkObject.transform.position, originalMilkPosition) > 0.01f)
        {
            milkObject.transform.position = Vector3.Lerp(milkObject.transform.position, originalMilkPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        milkObject.transform.position = originalMilkPosition;

        // Reset camera to original position and size smoothly
        while (Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalCameraSize, Time.deltaTime * zoomSpeed);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;

        // Reactivate draggable script
        if (draggableScript != null)
        {
            draggableScript.enabled = true;
        }

        // Set milk object animation to idle
        if (milkObjectAnimator != null)
        {
            milkObjectAnimator.SetTrigger("Idle");
        }

        // Camera slide re-activate
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = true;
        }
    }

    // Function to trigger milk pouring after ready animation ends (using AnimationEvent)
    public void StartMilkPouring()
    {
        if (milkObjectAnimator != null)
        {
            milkObjectAnimator.SetTrigger("MilkPouring");
        }
    }
}
