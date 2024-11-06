using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Transform coffeeSnapPoint; // 咖啡机上用于放杯子的目标位置
    public Transform cupBottom;
    public Transform cup;
    public Animator cupAnimator; // Animator，用于播放杯子的动画
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
        if (other.CompareTag("Cup") && other.transform == cup)
        {
            // 检查是否在 coffeeSnapPoint 附近
            if (Vector3.Distance(other.transform.position, coffeeSnapPoint.position) < 2f)
            {
                isCupSnapped = true; // 杯子进入 coffeeSnapPoint 范围，设置 isCupSnapped 为 true
                Debug.Log("Cup snapped into snap point");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cup") && other.transform == cup)
        {
            isCupSnapped = false; // 杯子离开 coffeeSnapPoint 范围，设置 isCupSnapped 为 false
            Debug.Log("Cup unsnapped from snap point");
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;

            // 播放制作咖啡动画
            if (cupAnimator != null)
            {
                cupAnimator.SetTrigger("StartCoffeeMaking");
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
