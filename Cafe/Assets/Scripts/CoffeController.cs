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

        SpriteRenderer pitcherSpriteRenderer = other.GetComponent<SpriteRenderer>();
        if (other.CompareTag(pitcherTag) && pitcherSpriteRenderer != null)
        {
            // Cappuccino logic
            if (pitcherSpriteRenderer.sprite.name == "pitcher wiz foam 2")
            {
                if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "coffee5")
                {
                    cupAnimator.SetTrigger("Cappucino");  // make cappucino
                    pitcherSpriteRenderer.sprite = originalPitcherSprite;
                    Debug.Log("Pitcher sprite changed back to: " + originalPitcherSprite.name);
                }
                else
                {
                    Debug.Log("Current cup sprite is: " + cupSpriteRenderer.sprite.name);
                    Debug.Log("Cup sprite is not coffee5, cannot make Cappuccino.");
                }
            }

            // Chocolate and Matcha logic
            if (pitcherSpriteRenderer.sprite.name == "pitcher wiz milk 2")
            {
                if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "chocolatecup")
                {
                    cupAnimator.SetTrigger("Chocolate"); // make chocolate
                    pitcherSpriteRenderer.sprite = originalPitcherSprite;
                    Debug.Log("Pitcher sprite changed back to: " + originalPitcherSprite.name);
                }
                else if (cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "matchacup")
                {
                    cupAnimator.SetTrigger("Matcha"); // make matcha
                    pitcherSpriteRenderer.sprite = originalPitcherSprite;
                    Debug.Log("Pitcher sprite changed back to: " + originalPitcherSprite.name);
                }
                else
                {
                    Debug.Log("Current cup sprite is: " + cupSpriteRenderer.sprite.name);
                    Debug.Log("Cup sprite is not matchacup or chocolatecup, cannot make drink.");
                }
            }
        }
    }

}
