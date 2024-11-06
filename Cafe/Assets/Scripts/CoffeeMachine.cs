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
    public float cupSnapThreshold = 3f; // 距离阈值，用于确定 cup 是否在正确的位置
    public bool isProducing = false;
    public bool isCupSnapped = false;

    private void Update()
    {
        // 检查 cup 是否位于 coffeeSnapPoint 附近
        if (Vector3.Distance(cup.position, coffeeSnapPoint.position) <= cupSnapThreshold)
        {
            Debug.Log("The distance" + Vector3.Distance(cup.position, coffeeSnapPoint.position));
            isCupSnapped = true; // 如果杯子在正确位置，设置 isCupSnapped 为 true
        }
        else
        {
            Debug.Log("The distance" + Vector3.Distance(cup.position, coffeeSnapPoint.position));
            isCupSnapped = false; // 如果杯子不在正确位置，设置 isCupSnapped 为 false
        }

        // if mouse on coffee machine
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("Hit detected with: " + hit.collider.gameObject.name);
        }

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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 如果点击鼠标左键，检查咖啡杯和 holder 是否在正确位置
            if (isCupSnapped && IsHolderInPosition())
            {
                StartCoffeeProduction();
                Debug.Log("Coffee making");
            }
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
