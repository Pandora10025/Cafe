using System.Collections;
using UnityEngine;

public class ChocolatePowderController : MonoBehaviour
{
    public SpriteRenderer chocolatePowderSpriteRenderer;  // Chocolate powder's SpriteRenderer
    public Sprite chocolatePowderHighlight;  // Highlighted version of the chocolate powder sprite
    public Sprite chocolatePowderOpened;  // Opened version of the chocolate powder sprite
    public Sprite originalChocolatePowder;  // Original version of the chocolate powder sprite
    public Animator cupAnimator;  // Animator for the cup

    public AudioSource OpenLid;

    private void Start()
    {
        if (chocolatePowderSpriteRenderer == null)
        {
            chocolatePowderSpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnMouseEnter()
    {
        // When the mouse hovers over the chocolate powder, change the sprite to highlighted version
        if (chocolatePowderSpriteRenderer != null)
        {
            chocolatePowderSpriteRenderer.sprite = chocolatePowderHighlight;
        }
    }

    private void OnMouseExit()
    {
        // When the mouse leaves the chocolate powder, change the sprite back to the original version
        if (chocolatePowderSpriteRenderer != null)
        {
            chocolatePowderSpriteRenderer.sprite = originalChocolatePowder;
        }
    }

    private void OnMouseDown()
    {
        // When the chocolate powder is clicked
        if (chocolatePowderSpriteRenderer != null)
        {
            // Change the chocolate powder sprite to the opened version
            chocolatePowderSpriteRenderer.sprite = chocolatePowderOpened;
            OpenLid.Play();
        }

        // Trigger the cup animation to change its current animation to "chocolate"
        if (cupAnimator != null)
        {
            cupAnimator.SetTrigger("AddChocolate");
        }

        // Wait 1 second, then change the sprite back to the original version
        StartCoroutine(ResetChocolatePowderSprite());
    }

    private IEnumerator ResetChocolatePowderSprite()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Change the chocolate powder sprite back to the original version
        if (chocolatePowderSpriteRenderer != null)
        {
            chocolatePowderSpriteRenderer.sprite = originalChocolatePowder;
        }
    }
}
