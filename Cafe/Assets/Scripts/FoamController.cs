using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MilkFoamMaker : MonoBehaviour
{
    public GameObject pitcher;  // pitcher with milk �Ķ���
    public GameObject steamWand;        // steam wand �Ķ���
    public GameObject pointer;          // pointer UI
    public GameObject progressBar;      // progress bar UI ����
    public float pointerSpeed = 1.0f;   // pointer �ƶ��ٶ�
    public float perfectStartX = 15.9f;  // ��������Ŀ�ʼ x ����
    public float perfectEndX = 57.3f;    // ��������Ľ��� x ����
    public float progressEndX = 116.3f;   // progress bar ����λ�õ� x ����

    public Sprite pitcherWithFoamSprite;  // �����ݵ� pitcher sprite
    public Sprite pitcherWithMilkSprite;

    private RectTransform pointerRectTransform;  // pointer �� RectTransform
    private RectTransform progressBarRectTransform;  // progress bar �� RectTransform
    private bool isFoaming = false;  // �Ƿ�������������

    private void Start()
    {
        pointerRectTransform = pointer.GetComponent<RectTransform>();
        progressBarRectTransform = progressBar.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isFoaming)
        {
            // Pointer �ƶ��߼�
            pointerRectTransform.anchoredPosition += new Vector2(pointerSpeed * Time.deltaTime, 0);

            float pointerX = pointerRectTransform.anchoredPosition.x;

            // ����Ƿ�����������
            if (pointerX >= perfectStartX && pointerX <= perfectEndX)
            {
                Debug.Log("Perfect!");
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;
            }

            // ��� pointer �ƶ��� progressBar ĩ��
            if (pointerX >= progressEndX)
            {
                StopFoamingProcess();  // ֹͣ��������
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;  // ����Ϊ�����ݵ� pitcher
                Debug.Log("Finished foaming!");
            }

            // �������Ƿ��ֶ��ƿ� pitcher
            if (Input.GetMouseButtonDown(0))
            {
                StopFoamingProcess();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide with pitcher");

        // ��� pitcher �Ƿ��� steam wand ��ײ���� sprite �� pitcher wiz milk
        if (other.gameObject == pitcher && pitcher.GetComponent<SpriteRenderer>().sprite == pitcherWithMilkSprite)
        {
            StartFoamingProcess();  // ��ʼ��������
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // �� pitcher �뿪 steam wand ʱ��ֹͣ��������
        if (other.gameObject == pitcher)
        {
            StopFoamingProcess();
        }
    }

    private void StartFoamingProcess()
    {
        if (!isFoaming)
        {
            isFoaming = true;
            pointer.SetActive(true);  // ���� pointer
            progressBar.SetActive(true);  // ���� progress bar
            pointerRectTransform.anchoredPosition = new Vector2(0, pointerRectTransform.anchoredPosition.y);  // ��ʼ�� pointer λ��
        }
    }

    private void StopFoamingProcess()
    {
        isFoaming = false;
        pointer.SetActive(false);  // ���� pointer
        progressBar.SetActive(false);  // ���� progress bar
    }
}
