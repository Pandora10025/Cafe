using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject;
    public GameObject cameraFocusPoint;  // The point to focus the camera on
    public SpriteRenderer pitcherSpriteRenderer;
    public Sprite milkSprite;
    public Sprite gmilkSprite; // Highlighted version of the milk sprite

    public Camera mainCamera;  // The main camera
    public float zoomInSize = 2f;  // Target size for zoom in
    public float zoomSpeed;   // Speed of the zoom transition

    private bool isZoomedIn = false;  // Whether the camera is zoomed in
    private float originalCameraSize;
    private Vector3 originalCameraPosition;

    void Start()
    {
        originalCameraSize = mainCamera.orthographicSize;
        originalCameraPosition = mainCamera.transform.position;
    }

    void Update()
    {
        // 检查鼠标是否悬停在 milkObject 上
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit && hit.collider != null && hit.collider.gameObject == milkObject)
        {
            // 获取 milkObject 的 SpriteRenderer 并将 sprite 改为 gmilkSprite
            SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
            if (milkSpriteRenderer != null)
            {
                milkSpriteRenderer.sprite = gmilkSprite;
            }

            // 只有当鼠标与 milkObject 碰撞时才触发 zoom in
            if (Input.GetMouseButtonDown(0) && !isZoomedIn)
            {
                StartCoroutine(ZoomInCamera());
            }
        }
        else
        {
            // 恢复原始 sprite，当鼠标不悬停时
            if (!isZoomedIn)
            {
                SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
                if (milkSpriteRenderer != null)
                {
                    milkSpriteRenderer.sprite = milkSprite;
                }
            }
        }

        // 按下鼠标右键时触发 zoom out
        if (Input.GetMouseButtonDown(1) && isZoomedIn)
        {
            StartCoroutine(ZoomOutCamera());
        }
    }


    private IEnumerator ZoomInCamera()
    {
        isZoomedIn = true;
        Vector3 targetPosition = new Vector3(cameraFocusPoint.transform.position.x, cameraFocusPoint.transform.position.y, mainCamera.transform.position.z);

        // 禁用 Camera_Slide 脚本
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = false;
        }

        // 平滑缩放并移动摄像机
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

        // 平滑缩放并移动摄像机回到原始状态
        while (Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalCameraSize, Time.deltaTime * zoomSpeed);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        // 确保最后值完全恢复到原始值
        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;

        // 重新启用 Camera_Slide 脚本
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = true;
        }
    }
}
