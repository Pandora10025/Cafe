using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject coffeeSprite; // ���ȵ� Sprite
    public Transform coffeeSnapPoint; // CoffeeSnap ��λ��
    public Transform cupBottom; // ���ӵײ���λ��
    public float stretchDuration = 0.1f; // �쳤��ʱ�䣬���ٵ�0.5���Լӿ��ٶ�
    private bool isProducing = false; // �Ƿ�������������
    private bool isCupSnapped = false; // �����Ƿ��Ѷ���

    private void Update()
    {
        if (isProducing)
        {
            coffeeSprite.transform.position = cupBottom.position; // �ÿ����汭���ƶ�
        }

        // ����Ұ������ʱ������������
        if (Input.GetMouseButtonDown(0) && isCupSnapped)
        {
            StartCoffeeProduction();
            Debug.Log("Coffee making");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTriggerStay2D triggered with: " + other.name);
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = true; // ������ CoffeeSnap ��Χ��
            Debug.Log("Cup is ready");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Not Collide");
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = false; // �����뿪 CoffeeSnap ��Χ
            Debug.Log("Cup is not ready");
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            coffeeSprite.SetActive(true); // ����� Sprite

            // ���� coffeeSprite �ĳ�ʼλ��Ϊ���ӵײ�
            coffeeSprite.transform.position = cupBottom.position;

            StartCoroutine(ProduceCoffee());
            Debug.Log("Coffee making");
        }
    }

    private IEnumerator ProduceCoffee()
    {
        Vector3 originalScale = coffeeSprite.transform.localScale;

        // ����Ŀ�� scale��ʹ���ȿ����쳤����
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y + 5.0f, originalScale.z); // ���Ӹ�����쳤����

        float stretchSpeed = 10.0f; // ������ٶȣ���ֵԽ������Խ��
        float elapsedTime = 0f;

        while (elapsedTime < stretchDuration)
        {
            // ͨ�����Բ�ֵ�������� scale
            coffeeSprite.transform.localScale = Vector3.Lerp(coffeeSprite.transform.localScale, targetScale, stretchSpeed * Time.deltaTime);

            // ����ʱ�����
            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡
        }

        // ȷ������ scale �ﵽĿ��ֵ
        coffeeSprite.transform.localScale = targetScale;

        isProducing = false;
    }


}
