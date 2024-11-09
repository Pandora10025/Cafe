using System.Collections;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject;
    public GameObject cameraFocusPoint;
    public SpriteRenderer pitcherSpriteRenderer;
    public Sprite originalPitcherSprite;
    public Sprite pitcherWithMilk1;
    public Sprite pitcherWithMilk2;
    public Animator milkObjectAnimator;
    public Camera mainCamera;
    public float zoomInSize = 3f;
    public float zoomSpeed;
    public Vector3 targetMilkPosition;
    private Vector3 originalMilkPosition;
    private float originalCameraSize;
    private Vector3 originalCameraPosition;

    private float sKeyPressedTime = 0f;
    private bool isZoomedIn = false;
    private Coroutine currentCoroutine;
    private enum MilkState { MouseHover, ZoomIn, Pour, Back }
    private MilkState currentState;

    void Start()
    {
        originalCameraSize = mainCamera.orthographicSize;
        originalCameraPosition = mainCamera.transform.position;
        originalMilkPosition = milkObject.transform.position;
        currentState = MilkState.MouseHover;  // ³õÊ¼×´Ì¬
    }

    void Update()
    {
        switch (currentState)
        {
            case MilkState.MouseHover:
                HandleMouseHover();
                break;
            case MilkState.ZoomIn:
                HandleZoomIn();
                break;
            case MilkState.Pour:
                HandlePour();
                break;
            case MilkState.Back:
                HandleBack();
                break;
        }
    }

    private void HandleMouseHover()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == milkObject)
        {
            if (milkObjectAnimator != null && !milkObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlight"))
            {
                milkObjectAnimator.SetTrigger("Highlight");
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (pitcherSpriteRenderer.sprite == originalPitcherSprite)
                {
                    currentState = MilkState.ZoomIn;
                }
            }
        }
        else
        {
            if (milkObjectAnimator != null && !milkObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                milkObjectAnimator.SetTrigger("Idle");
            }
        }
    }

    private void HandleZoomIn()
    {
        if (!isZoomedIn)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ZoomInCamera());
        }

        milkObject.transform.position = Vector3.Lerp(milkObject.transform.position, targetMilkPosition, Time.deltaTime * zoomSpeed);

        if (Vector3.Distance(milkObject.transform.position, targetMilkPosition) < 0.01f)
        {
            currentState = MilkState.Pour;
        }
    }

    private void HandlePour()
    {
        if (Input.GetKey(KeyCode.S))
        {
            sKeyPressedTime += Time.deltaTime;

            if (milkObjectAnimator != null && sKeyPressedTime < 2f && !milkObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName("MilkReady"))
            {
                milkObjectAnimator.SetTrigger("MilkReady");
            }
            else if (sKeyPressedTime >= 2f && sKeyPressedTime < 3f && pitcherSpriteRenderer.sprite != pitcherWithMilk1)
            {
                pitcherSpriteRenderer.sprite = pitcherWithMilk1;
            }
            else if (sKeyPressedTime >= 3f && pitcherSpriteRenderer.sprite != pitcherWithMilk2)
            {
                pitcherSpriteRenderer.sprite = pitcherWithMilk2;
                currentState = MilkState.Back;
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            sKeyPressedTime = 0f;
            if (milkObjectAnimator != null && !milkObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName("MilkBack"))
            {
                milkObjectAnimator.SetTrigger("MilkBack");
            }
        }
    }

    private void HandleBack()
    {
        if (milkObjectAnimator != null && !milkObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName("MilkBack"))
        {
            milkObjectAnimator.SetTrigger("MilkBack");
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ResetMilkAndZoomOut());

        pitcherSpriteRenderer.sprite = originalPitcherSprite;
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


    private IEnumerator ResetMilkAndZoomOut()
    {
        // Move milk object back to original position
        while (Vector3.Distance(milkObject.transform.position, originalMilkPosition) > 0.01f)
        {
            milkObject.transform.position = Vector3.Lerp(milkObject.transform.position, originalMilkPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        milkObject.transform.position = originalMilkPosition;

        // Reset camera to original position and size
        while (Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalCameraSize, Time.deltaTime * zoomSpeed);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;

        // Set milk object animation to idle
        if (milkObjectAnimator != null && !milkObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            milkObjectAnimator.SetTrigger("Idle");
        }

        isZoomedIn = false;
        currentState = MilkState.MouseHover;  // ·µ»Øµ½×î³õ×´Ì¬
    }
}
