using UnityEngine;

public class PitcherDraggableController : MonoBehaviour
{
    public Camera mainCamera;  // The main camera
    private Draggable draggableScript;  // Reference to the draggable script
    private float originalCameraSize;
    private bool isDraggableEnabled; // To keep track of the current state

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
            isDraggableEnabled = true;
        }
    }

    void Update()
    {
        if (mainCamera.orthographicSize != originalCameraSize && draggableScript != null && isDraggableEnabled)
        {
            // Disable draggable script when camera size is not the original size
            draggableScript.enabled = false;
            isDraggableEnabled = false;
        }
        else if (mainCamera.orthographicSize == originalCameraSize && draggableScript != null && !isDraggableEnabled)
        {
            // Enable draggable script when camera size returns to the original size
            draggableScript.enabled = true;
            isDraggableEnabled = true;
        }
    }
}
