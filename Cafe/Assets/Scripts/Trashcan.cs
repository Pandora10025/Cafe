using UnityEngine;

public class TrashCanController : MonoBehaviour
{
    public Sprite originalPitcherSprite; // The original sprite for the pitcher
    public CoffeeMachine coffeeMachine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Cup"
        if (other.CompareTag("Cup"))
        {
            Animator cupAnimator = other.GetComponent<Animator>();
            if (cupAnimator != null)
            {
                // Reset all triggers to ensure the animation state is completely cleared
                cupAnimator.ResetTrigger("StartCoffeeMaking");
              
                cupAnimator.SetTrigger("BackToIdle");

                // Reset coffee machine state
                if (coffeeMachine != null)
                {
                    coffeeMachine.isProducing = false;
                    coffeeMachine.isCupSnapped = false;
                }

                Debug.Log("Cup animation reset to idle and coffee machine state reset");
            }
        }
        // Check if the colliding object is tagged as "Pitcher"
        else if (other.CompareTag("Pitcher"))
        {
            SpriteRenderer pitcherSpriteRenderer = other.GetComponent<SpriteRenderer>();
            if (pitcherSpriteRenderer != null)
            {
                // Set the pitcher sprite back to the original sprite
                pitcherSpriteRenderer.sprite = originalPitcherSprite;
                Debug.Log("Pitcher sprite reset to original");
            }
        }
    }
}
