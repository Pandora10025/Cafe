using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject; // milk object
    public SpriteRenderer pitcherSpriteRenderer; // pitcher sprite renderer
    public Sprite pitcherWithMilkSprite; // pitcher with milk sprite
    public float tiltAngle = 20f; // ��б�Ƕ�
    public float tiltSpeed = 2f; // ��б�ٶ�
    private bool isTilted = false; // ��ǰ�Ƿ�����б״̬
    private Quaternion originalRotation; // ��¼ milk object ��ԭʼ��ת

    void Start()
    {
        originalRotation = milkObject.transform.rotation; // ��¼ milk �ĳ�ʼ��ת
    }

    void Update()
    {
        // ��� milk object ������б
        if (Input.GetMouseButtonDown(0))
        {
            // �������Ƿ������� milk object
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit && hit.collider != null && hit.collider.gameObject == milkObject)
            {
                if (!isTilted)
                {
                    // ���û����б����б milk object
                    StartCoroutine(TiltMilk(milkObject.transform.rotation, Quaternion.Euler(0, 0, -tiltAngle)));
                }
                else
                {
                    // ����Ѿ���б�ˣ��ָ�ԭʼλ�ã����л� pitcher sprite
                    StartCoroutine(TiltMilk(milkObject.transform.rotation, originalRotation));
                    pitcherSpriteRenderer.sprite = pitcherWithMilkSprite; // �л�����ţ�̵� pitcher
                }

                isTilted = !isTilted; // �л�״̬
            }
        }
    }

    // ʹ�� Lerp ��������б��ָ� milk object
    private IEnumerator TiltMilk(Quaternion startRotation, Quaternion targetRotation)
    {
        float elapsedTime = 0f;

        while (elapsedTime < tiltSpeed)
        {
            milkObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / tiltSpeed);
            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡
        }

        // ȷ��������ת��λ
        milkObject.transform.rotation = targetRotation;
    }
}
