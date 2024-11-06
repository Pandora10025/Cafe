using System.Collections;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    public GameObject orderCappuccino;   // Order Cappuccino Object
    public GameObject thankYou;          // Thank You Object
    public GameObject customer;          // Customer Object
    public Sprite NcupSprite;
    public CoffeeMachine coffeeMachine;
    public float thankYouDuration = 2.0f; // Duration for showing the Thank You message
    public float customerMoveDistance = 5.0f; // Distance customer moves to the left
    public float customerMoveSpeed = 2.0f;   // Speed at which customer moves
    public float customerReturnDelay = 3.0f; // Delay before customer returns
    public Animator cupAnimator;        // Animator for the cup

    private Vector3 customerStartPos;     // Customer's original position
    private bool isProcessing = false;    // To prevent multiple triggers

    private void Start()
    {
        // Store the starting position of the customer
        customerStartPos = customer.transform.position;

        // Ensure Thank You is deactivated at the start
        thankYou.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the cup with the cappuccino sprite
        SpriteRenderer cupSpriteRenderer = other.GetComponent<SpriteRenderer>();
        if (other.gameObject.CompareTag("Cup") && cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == "cappuccino")
        {
            if (!isProcessing)
            {
                StartCoroutine(ProcessOrder(cupSpriteRenderer));
            }
        }
    }

    private IEnumerator ProcessOrder(SpriteRenderer cupSpriteRenderer)
    {
        isProcessing = true;

        // Step 1: Deactivate the current order
        orderCappuccino.SetActive(false);

        // Step 2: Activate the Thank You message
        thankYou.SetActive(true);
        Debug.Log("Thank You!");

        // Step 3: Wait for a few seconds
        yield return new WaitForSeconds(thankYouDuration);

        // Step 4: Deactivate the Thank You message
        thankYou.SetActive(false);

        // Step 5: Change the cup's sprite to Ncup and enable cupAnimator
        cupSpriteRenderer.sprite = NcupSprite;
        Debug.Log("Cup sprite changed to Ncup");

        if (cupAnimator != null)
        {
            cupAnimator.enabled = true; // Enable the cup's animator
            cupAnimator.SetTrigger("BackToIdle"); // Trigger the Idle animation
            Debug.Log("Cup animator enabled and BackToIdle trigger set");
        }

        CoffeeMachine coffeeMachineScript = coffeeMachine.GetComponent<CoffeeMachine>();
        coffeeMachineScript.isProducing = false;   // Reset producing state
        coffeeMachineScript.isCupSnapped = false;  // Allow the cup to be snapped again

        // Step 6: Move the customer to the left
        Vector3 targetPosition = customerStartPos + Vector3.left * customerMoveDistance;
        while (Vector3.Distance(customer.transform.position, targetPosition) > 0.1f)
        {
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPosition, customerMoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Step 7: Wait for a delay before the customer returns
        yield return new WaitForSeconds(customerReturnDelay);

        // Step 8: Move the customer back to the starting position
        while (Vector3.Distance(customer.transform.position, customerStartPos) > 0.1f)
        {
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, customerStartPos, customerMoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Step 9: Activate the order object again
        orderCappuccino.SetActive(true);

        // Reset the processing flag
        isProcessing = false;
    }
}
