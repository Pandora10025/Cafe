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

        // ����Ƿ��� Pitcher ��ײ���� Pitcher �� Sprite Ϊ "pitcher wiz foam 2"
        SpriteRenderer pitcherSpriteRenderer = other.GetComponent<SpriteRenderer>();
        if (other.CompareTag(pitcherTag) && pitcherSpriteRenderer != null && pitcherSpriteRenderer.sprite.name == "pitcher wiz foam 2")
        {
            // ��������Ƿ�Ϊ "coffee5" sprite
            if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "coffee5")
            {
                MakeCappuccino();  // ��������������㣬����������ŵ
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
        // ���� Animator �Է�ֹ���� Sprite ����
        if (cupAnimator != null)
        {
            cupAnimator.enabled = false;
            Debug.Log("Cup Animator disabled to change sprite.");
        }

        // �����ӵ� sprite ���� cappuccino �� sprite
        if (cupSpriteRenderer != null)
        {
            cupSpriteRenderer.sprite = cappuccinoSprite;
            Debug.Log("Cup sprite changed to: " + cupSpriteRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("Cup does not have a SpriteRenderer component.");
        }

        // �������Ҫ�������� Animator��ȷ�������������� Sprite
        // cupAnimator.enabled = true;
    }
}
