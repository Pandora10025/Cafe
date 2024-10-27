using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject coffeeSprite; // 咖啡的 Sprite
    public Transform coffeeSnapPoint; // CoffeeSnap 的位置
    public Transform cupBottom; // 杯子底部的位置
    public float stretchDuration = 0.1f; // 伸长的时间，减少到0.5秒以加快速度
    private bool isProducing = false; // 是否正在生产咖啡
    private bool isCupSnapped = false; // 杯子是否已对齐

    private void Update()
    {
        if (isProducing)
        {
            coffeeSprite.transform.position = cupBottom.position; // 让咖啡随杯子移动
        }

        // 当玩家按下鼠标时触发咖啡生产
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
            isCupSnapped = true; // 杯子在 CoffeeSnap 范围内
            Debug.Log("Cup is ready");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Not Collide");
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = false; // 杯子离开 CoffeeSnap 范围
            Debug.Log("Cup is not ready");
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;
            coffeeSprite.SetActive(true); // 激活咖啡 Sprite

            // 设置 coffeeSprite 的初始位置为杯子底部
            coffeeSprite.transform.position = cupBottom.position;

            StartCoroutine(ProduceCoffee());
            Debug.Log("Coffee making");
        }
    }

    private IEnumerator ProduceCoffee()
    {
        Vector3 originalScale = coffeeSprite.transform.localScale;

        // 调整目标 scale，使咖啡可以伸长更多
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y + 5.0f, originalScale.z); // 增加更多的伸长幅度

        float stretchSpeed = 10.0f; // 拉伸的速度，数值越大，拉伸越快
        float elapsedTime = 0f;

        while (elapsedTime < stretchDuration)
        {
            // 通过线性插值快速增加 scale
            coffeeSprite.transform.localScale = Vector3.Lerp(coffeeSprite.transform.localScale, targetScale, stretchSpeed * Time.deltaTime);

            // 增加时间计数
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        // 确保最后的 scale 达到目标值
        coffeeSprite.transform.localScale = targetScale;

        isProducing = false;
    }


}
