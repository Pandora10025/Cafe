using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Transform coffeeSnapPoint; // ���Ȼ������ڷű��ӵ�Ŀ��λ��
    public Transform cupBottom;
    public Transform cup;
    public Animator cupAnimator; // Animator�����ڲ��ű��ӵĶ���
    public Transform holder; // Reference to the holder object
    public Transform holderPoint; // Reference to the holder's target point
    public float holderSnapThreshold = 0.1f; // ������ֵ������ȷ�� holder �Ƿ�����ȷ��λ��
    public bool isProducing = false;
    public bool isCupSnapped = false;

    private void Update()
    {
        // if mouse on coffee machine
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(0))
        {
            // ����Ƿ������������ȵ�����
            Debug.Log("Mouse clicked on CoffeeMachine");
            if (isCupSnapped)
            {
                Debug.Log("Cup is snapped in place.");
            }
            if (IsHolderInPosition())
            {
                Debug.Log("Holder is in position.");
            }

            if (isCupSnapped && IsHolderInPosition())
            {
                StartCoffeeProduction();
                Debug.Log("Coffee making");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cup") && other.transform == cup)
        {
            // ����Ƿ��� coffeeSnapPoint ����
            if (Vector3.Distance(other.transform.position, coffeeSnapPoint.position) < 2f)
            {
                isCupSnapped = true; // ���ӽ��� coffeeSnapPoint ��Χ������ isCupSnapped Ϊ true
                Debug.Log("Cup snapped into snap point");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cup") && other.transform == cup)
        {
            isCupSnapped = false; // �����뿪 coffeeSnapPoint ��Χ������ isCupSnapped Ϊ false
            Debug.Log("Cup unsnapped from snap point");
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;

            // �����������ȶ���
            if (cupAnimator != null)
            {
                cupAnimator.SetTrigger("StartCoffeeMaking");
            }

            Debug.Log("Coffee making started");
        }
    }

    // ��� holder �Ƿ�����ȷ��λ����
    private bool IsHolderInPosition()
    {
        if (holder == null || holderPoint == null)
        {
            return false;
        }

        float distance = Vector3.Distance(holder.position, holderPoint.position);
        return distance <= holderSnapThreshold;
    }
}
