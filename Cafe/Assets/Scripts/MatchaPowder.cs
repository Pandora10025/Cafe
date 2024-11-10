using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchaPowder : MonoBehaviour
{
    public SpriteRenderer matchaPowderSpriteRenderer;  // Chocolate powder's SpriteRenderer
    public Sprite matchaPowderHighlight;  // Highlighted version of the chocolate powder sprite
    public Sprite matchaPowderOpened;  // Opened version of the chocolate powder sprite
    public Sprite originalMatchaPowder;  // Original version of the chocolate powder sprite
    public Animator cupAnimator;  // Animator for the cup

    private void Start()
    {
        if (matchaPowderSpriteRenderer == null)
        {
            matchaPowderSpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnMouseEnter()
    {
        // When the mouse hovers over the chocolate powder, change the sprite to highlighted version
        if (matchaPowderSpriteRenderer != null)
        {
            matchaPowderSpriteRenderer.sprite = matchaPowderHighlight;
        }
    }

    private void OnMouseExit()
    {
        // When the mouse leaves the chocolate powder, change the sprite back to the original version
        if (matchaPowderSpriteRenderer != null)
        {
            matchaPowderSpriteRenderer.sprite = matchaPowderHighlight;
        }
    }

    private void OnMouseDown()
    {
        // When the chocolate powder is clicked
        if (matchaPowderSpriteRenderer != null)
        {
            // Change the chocolate powder sprite to the opened version
            matchaPowderSpriteRenderer.sprite = matchaPowderOpened;
        }

        // Trigger the cup animation to change its current animation to "chocolate"
        if (cupAnimator != null)
        {
            cupAnimator.SetTrigger("AddMatcha");
        }

        // Wait 1 second, then change the sprite back to the original version
        StartCoroutine(ResetMatchaPowderSprite());
    }

    private IEnumerator ResetMatchaPowderSprite()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Change the chocolate powder sprite back to the original version
        if (matchaPowderSpriteRenderer != null)
        {
            matchaPowderSpriteRenderer.sprite = originalMatchaPowder;
        }
    }
}
