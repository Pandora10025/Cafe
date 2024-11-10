using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    public Sprite cappuccinoSprite;  // Sprite for cappuccino
    public Sprite chocolateSprite;
    public Sprite matchaSprite;
    public SpriteRenderer cupSpriteRenderer;  // SpriteRenderer for the cup to change the sprite
    public string pitcherTag = "Pitcher"; // Tag for the pitcher
    public Animator cupAnimator; // Animator for the cup
    public Sprite originalPitcherSprite; // Original sprite for the pitcher

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
                MakeCappuccino(pitcherSpriteRenderer);  // make cappucino
            }
            else
            {
                Debug.Log("Current cup sprite is: " + cupSpriteRenderer.sprite.name);
                Debug.Log("Cup sprite is not coffee5, cannot make Cappuccino.");
            }
        }

        if (other.CompareTag(pitcherTag) && pitcherSpriteRenderer != null && pitcherSpriteRenderer.sprite.name == "pitcher wiz milk 2")
        {
            // 检查自身是否为 "chocoalte" sprite
            if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "chocolatecup")
            {
                MakeChocolate(pitcherSpriteRenderer);  // make chocolate
            }
            else
            {
                Debug.Log("Current cup sprite is: " + cupSpriteRenderer.sprite.name);
                Debug.Log("Cup sprite is not chocolatecup, cannot make chocolate.");
            }
        }

        if (other.CompareTag(pitcherTag) && pitcherSpriteRenderer != null && pitcherSpriteRenderer.sprite.name == "pitcher wiz milk 2")
        {
            // 检查自身是否为 "matcha" sprite
            if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "matchacup")
            {
                MakeMatcha(pitcherSpriteRenderer);  // make chocolate
            }
            else
            {
                Debug.Log("Current cup sprite is: " + cupSpriteRenderer.sprite.name);
                Debug.Log("Cup sprite is not chocolatecup, cannot make matcha.");
            }
        }
    }

    private void MakeCappuccino(SpriteRenderer pitcherSpriteRenderer)
    {
        // not animator
        if (cupAnimator != null)
        {
            cupAnimator.enabled = false;
            Debug.Log("Cup Animator disabled to change sprite.");
        }

        // cup-->cappucino
        if (cupSpriteRenderer != null)
        {
            cupSpriteRenderer.sprite = cappuccinoSprite;
            Debug.Log("Cup sprite changed to: " + cupSpriteRenderer.sprite.name);
        }


        // pitcher bakc to original
        if (pitcherSpriteRenderer != null && originalPitcherSprite != null)
        {
            pitcherSpriteRenderer.sprite = originalPitcherSprite;
            Debug.Log("Pitcher sprite changed back to: " + originalPitcherSprite.name);
        }

    }
    private void MakeChocolate(SpriteRenderer pitcherSpriteRenderer)
    {
        // not animator
        if (cupAnimator != null)
        {
            cupAnimator.enabled = false;
            Debug.Log("Cup Animator disabled to change sprite.");
        }

        // cup-->chocolate
        if (cupSpriteRenderer != null)
        {
            cupSpriteRenderer.sprite = chocolateSprite;
            Debug.Log("Cup sprite changed to: " + cupSpriteRenderer.sprite.name);
        }


        // pitcher bakc to original
        if (pitcherSpriteRenderer != null && originalPitcherSprite != null)
        {
            pitcherSpriteRenderer.sprite = originalPitcherSprite;
            Debug.Log("Pitcher sprite changed back to: " + originalPitcherSprite.name);
        }

    }

    private void MakeMatcha(SpriteRenderer pitcherSpriteRenderer)
    {
        // not animator
        if (cupAnimator != null)
        {
            cupAnimator.enabled = false;
            Debug.Log("Cup Animator disabled to change sprite.");
        }

        // cup-->matcha
        if (cupSpriteRenderer != null)
        {
            cupSpriteRenderer.sprite = matchaSprite;
            Debug.Log("Cup sprite changed to: " + cupSpriteRenderer.sprite.name);
        }


        // pitcher bakc to original
        if (pitcherSpriteRenderer != null && originalPitcherSprite != null)
        {
            pitcherSpriteRenderer.sprite = originalPitcherSprite;
            Debug.Log("Pitcher sprite changed back to: " + originalPitcherSprite.name);
        }

    }
}
