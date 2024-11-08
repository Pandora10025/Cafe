using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Transform coffeeSnapPoint; // 咖啡机上用于放杯子的目标位置
    public Transform cupBottom;
    public Transform cup;
    public Animator cupAnimator; // Animator，用于播放杯子的动画
    public Animator coffeeMachineAnimator; // Animator，用于播放咖啡机的动画
    public Transform holder; // Reference to the holder object
    public Transform holderPoint; // Reference to the holder's target point
    public float holderSnapThreshold = 0.1f; // 距离阈值，用于确定 holder 是否在正确的位置
    public bool isProducing = false;
    public bool isCupSnapped = false;

    private void Update()
    {
        // if mouse on coffee machine
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(0))
        {
            // 检查是否满足制作咖啡的条件
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
            isCupSnapped = true; // 杯子进入咖啡机范围，设置 isCupSnapped 为 true
            Debug.Log("Cup snapped into position");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = false; // 杯子离开咖啡机范围，设置 isCupSnapped 为 false
            Debug.Log("Cup unsnapped from position");

            // 还原咖啡机的动画为 idle
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

            // 播放咖啡机的制作咖啡动画
            if (coffeeMachineAnimator != null)
            {
                coffeeMachineAnimator.SetTrigger("StartCoffeeMaking");
                Debug.Log("CoffeeMachine started coffee-making animation");
            }

            // 播放制作咖啡动画
            if (cupAnimator != null)
            {
                cupAnimator.SetTrigger("StartCoffeeMaking");
                Debug.Log("Cup started coffee-making animation");
            }

            Debug.Log("Coffee making started");
        }
    }

    // 检查 holder 是否在正确的位置上
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
