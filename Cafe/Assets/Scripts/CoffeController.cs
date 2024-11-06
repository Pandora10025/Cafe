using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    public Sprite cappuccinoSprite;  // Sprite for cappuccino
    public SpriteRenderer cupSpriteRenderer;  // SpriteRenderer for the cup to change the sprite
    public string pitcherTag = "Pitcher"; // Tag for the pitcher
    public Animator cupAnimator; // Animator for the cup

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);

        // 检查是否与 Pitcher 碰撞，且 Pitcher 的 Sprite 为 "pitcher wiz foam 2"
        SpriteRenderer pitcherSpriteRenderer = other.GetComponent<SpriteRenderer>();
        if (other.CompareTag(pitcherTag) && pitcherSpriteRenderer != null && pitcherSpriteRenderer.sprite.name == "pitcher wiz foam 2")
        {
            // 检查自身是否为 "coffee5" sprite
            if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "coffee5")
            {
                MakeCappuccino();  // 如果以上条件满足，制作卡布奇诺
            }
            else
            {
                Debug.Log("Current cup sprite is: " + cupSpriteRenderer.sprite.name);
                Debug.Log("Cup sprite is not coffee5, cannot make Cappuccino.");
            }
        }
    }

    private void MakeCappuccino()
    {
        // 禁用 Animator 以防止覆盖 Sprite 更改
        if (cupAnimator != null)
        {
            cupAnimator.enabled = false;
            Debug.Log("Cup Animator disabled to change sprite.");
        }

        // 将杯子的 sprite 换成 cappuccino 的 sprite
        if (cupSpriteRenderer != null)
        {
            cupSpriteRenderer.sprite = cappuccinoSprite;
            Debug.Log("Cup sprite changed to: " + cupSpriteRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("Cup does not have a SpriteRenderer component.");
        }

        // 如果有需要可以启用 Animator，确保不会立即覆盖 Sprite
        // cupAnimator.enabled = true;
    }
}
