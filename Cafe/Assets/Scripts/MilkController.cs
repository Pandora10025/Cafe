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
        // �������Ƿ���ͣ�� milkObject ��
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit && hit.collider != null && hit.collider.gameObject == milkObject)
        {
            // ��ȡ milkObject �� SpriteRenderer ���� sprite ��Ϊ gmilkSprite
            SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
            if (milkSpriteRenderer != null)
            {
                milkSpriteRenderer.sprite = gmilkSprite;
            }

            // ֻ�е������ milkObject ��ײʱ�Ŵ��� zoom in
            if (Input.GetMouseButtonDown(0) && !isZoomedIn)
            {
                StartCoroutine(ZoomInCamera());
            }
        }
        else
        {
            // �ָ�ԭʼ sprite������겻��ͣʱ
            if (!isZoomedIn)
            {
                SpriteRenderer milkSpriteRenderer = milkObject.GetComponent<SpriteRenderer>();
                if (milkSpriteRenderer != null)
                {
                    milkSpriteRenderer.sprite = milkSprite;
                }
            }
        }

        // ��������Ҽ�ʱ���� zoom out
        if (Input.GetMouseButtonDown(1) && isZoomedIn)
        {
            StartCoroutine(ZoomOutCamera());
        }
    }


    private IEnumerator ZoomInCamera()
    {
        isZoomedIn = true;
        Vector3 targetPosition = new Vector3(cameraFocusPoint.transform.position.x, cameraFocusPoint.transform.position.y, mainCamera.transform.position.z);

        // ���� Camera_Slide �ű�
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = false;
        }

        // ƽ�����Ų��ƶ������
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

        // ƽ�����Ų��ƶ�������ص�ԭʼ״̬
        while (Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalCameraSize, Time.deltaTime * zoomSpeed);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        // ȷ�����ֵ��ȫ�ָ���ԭʼֵ
        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;

        // �������� Camera_Slide �ű�
        Camera_Slide cameraSlide = mainCamera.GetComponent<Camera_Slide>();
        if (cameraSlide != null)
        {
            cameraSlide.enabled = true;
        }
    }
}
