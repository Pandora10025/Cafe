using UnityEngine;

public class PitcherDraggableController : MonoBehaviour
{
    public Camera mainCamera;  // The main camera
    private Draggable draggableScript;  // Reference to the draggable script
    private float originalCameraSize;

    void Start()
    {
        // Store the original camera size
        originalCameraSize = mainCamera.orthographicSize;

        // Get the draggable script attached to this GameObject
        draggableScript = GetComponent<Draggable>();

        // Ensure draggable script is initially enabled
        if (draggableScript != null)
        {
            draggableScript.enabled = true;
        }
    }

    void Update()
    {
        // Disable draggable script when camera size is less than the original size
        if (mainCamera.orthographicSize < originalCameraSize && draggableScript != null)
        {
            draggableScript.enabled = false;
        }
        // Enable draggable script when camera size returns to the original size
        else if (mainCamera.orthographicSize >= originalCameraSize && draggableScript != null)
        {
            draggableScript.enabled = true;
        }
    }
}
