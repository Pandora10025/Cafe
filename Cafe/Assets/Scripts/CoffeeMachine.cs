using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Transform coffeeSnapPoint; // ���Ȼ������ڷű��ӵ�Ŀ��λ��
    public Transform cupBottom;
    public Transform cup;
    public Animator cupAnimator; // Animator�����ڲ��ű��ӵĶ���
    public Animator coffeeMachineAnimator; // Animator�����ڲ��ſ��Ȼ��Ķ���
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
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = true; // ���ӽ��뿧�Ȼ���Χ������ isCupSnapped Ϊ true
            Debug.Log("Cup snapped into position");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = false; // �����뿪���Ȼ���Χ������ isCupSnapped Ϊ false
            Debug.Log("Cup unsnapped from position");

            // ��ԭ���Ȼ��Ķ���Ϊ idle
            if (coffeeMachineAnimator != null)
            {
                coffeeMachineAnimator.SetTrigger("Idle");
                Debug.Log("CoffeeMachine returned to idle animation");
            }
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;

            // ���ſ��Ȼ����������ȶ���
            if (coffeeMachineAnimator != null)
            {
                coffeeMachineAnimator.SetTrigger("StartCoffeeMaking");
                Debug.Log("CoffeeMachine started coffee-making animation");
            }

            // �����������ȶ���
            if (cupAnimator != null)
            {
                cupAnimator.SetTrigger("StartCoffeeMaking");
                Debug.Log("Cup started coffee-making animation");
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
