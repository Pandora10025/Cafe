using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject; // milk object
    public SpriteRenderer pitcherSpriteRenderer; // pitcher sprite renderer
    public Sprite pitcherWithMilkSprite; // pitcher with milk sprite
    public float tiltAngle = 20f; // 倾斜角度
    public float tiltSpeed = 2f; // 倾斜速度
    private bool isTilted = false; // 当前是否是倾斜状态
    private Quaternion originalRotation; // 记录 milk object 的原始旋转

    void Start()
    {
        originalRotation = milkObject.transform.rotation; // 记录 milk 的初始旋转
    }

    void Update()
    {
        // 点击 milk object 进行倾斜
        if (Input.GetMouseButtonDown(0))
        {
            // 检测鼠标是否点击到了 milk object
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit && hit.collider != null && hit.collider.gameObject == milkObject)
            {
                if (!isTilted)
                {
                    // 如果没有倾斜，倾斜 milk object
                    StartCoroutine(TiltMilk(milkObject.transform.rotation, Quaternion.Euler(0, 0, -tiltAngle)));
                }
                else
                {
                    // 如果已经倾斜了，恢复原始位置，并切换 pitcher sprite
                    StartCoroutine(TiltMilk(milkObject.transform.rotation, originalRotation));
                    pitcherSpriteRenderer.sprite = pitcherWithMilkSprite; // 切换成有牛奶的 pitcher
                }

                isTilted = !isTilted; // 切换状态
            }
        }
    }

    // 使用 Lerp 来缓慢倾斜或恢复 milk object
    private IEnumerator TiltMilk(Quaternion startRotation, Quaternion targetRotation)
    {
        float elapsedTime = 0f;

        while (elapsedTime < tiltSpeed)
        {
            milkObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / tiltSpeed);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        // 确保最后的旋转到位
        milkObject.transform.rotation = targetRotation;
    }
}
