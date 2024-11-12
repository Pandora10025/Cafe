using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    public GameObject order_prefab;  // Customer Order speech bubble prefab
    private GameObject currentOrderInstance;  // Reference to the current order instance

    [SerializeField]
    public List<ScriptOb_Drinks> drinks = new List<ScriptOb_Drinks>(); // Drinks scriptable object list

    public GameObject thankYou;          // Thank You Object
    public GameObject customer;          // Customer Object

    public Sprite NcupSprite;

    public CoffeeMachine coffeeMachine;

    public float thankYouDuration = 2.0f; // Duration for showing the Thank You message
    public float customerMoveDistance = 5.0f; // Distance customer moves to the left
    public float customerMoveSpeed = 2.0f;   // Speed at which customer moves
    public float customerReturnDelay = 3.0f; // Delay before customer returns

    public float pos_x;
    public float pos_y;
    public float pos_z;

    public Animator cupAnimator;        // Animator for the cup

    private Vector3 customerStartPos;     // Customer's original position
    private bool isProcessing = false;    // To prevent multiple triggers

    private void Start()
    {
        coffeeMachine = FindAnyObjectByType<CoffeeMachine>();
        // Store the starting position of the customer
        customerStartPos = customer.transform.position;

        // Ensure Thank You is deactivated at the start
        thankYou.SetActive(false);

        // Find cup in the scene
        GameObject cup = GameObject.FindGameObjectWithTag("Cup");

        // Assign animator for cup to cupAnimator
        cupAnimator = cup.GetComponent<Animator>();

        pos_x = this.transform.position.x;
        pos_y = this.transform.position.y;
        pos_z = this.transform.position.z;

        GenerateOrder();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the cup
        if (other.gameObject.CompareTag("Cup"))
        {
            // Get the sprite on the cup
            SpriteRenderer cupSpriteRenderer = other.GetComponent<SpriteRenderer>();
            if (cupSpriteRenderer != null && currentOrderInstance != null)
            {
                // Get the sprite on the current order instance
                SpriteRenderer orderSpriteRenderer = currentOrderInstance.GetComponent<SpriteRenderer>();

                if (orderSpriteRenderer != null)
                {
                    // Debug log for sprite comparison
                    Debug.Log("Cup sprite name: " + cupSpriteRenderer.sprite.name);
                    Debug.Log("Order sprite name: " + orderSpriteRenderer.sprite.name);

                    if (cupSpriteRenderer.sprite.name == orderSpriteRenderer.sprite.name)
                    {
                        if (!isProcessing)
                        {
                            StartCoroutine(ProcessOrder(cupSpriteRenderer));
                        }
                    }
                    else
                    {
                        Debug.Log("Order and cup sprite do not match.");
                    }
                }
                else
                {
                    Debug.LogError("Current order instance does not have a SpriteRenderer component.");
                }
            }
        }
    }

    public GameObject GenerateOrder()
    {
        int tempIndex = Random.Range(0, drinks.Count);
        Debug.Log("Generated order index: " + tempIndex);

        // Instantiate the order prefab and assign it to currentOrderInstance
        currentOrderInstance = Instantiate(order_prefab, new Vector3(pos_x + 6, pos_y + 3, pos_z), Quaternion.identity);

        // Set up the current order instance with the drink information
        ScriptOb_Drinks_Controller tempController = currentOrderInstance.GetComponent<ScriptOb_Drinks_Controller>();

        if (tempController != null && tempIndex < drinks.Count)
        {
            tempController.drink = drinks[tempIndex];
            tempController.SetUp();
        }
        else
        {
            Debug.LogError("Error setting up the order instance.");
        }

        return currentOrderInstance;
    }

    private IEnumerator ProcessOrder(SpriteRenderer cupSpriteRenderer)
    {
        isProcessing = true;

        // Step 1: Deactivate the current order
        if (currentOrderInstance != null)
        {
            currentOrderInstance.SetActive(false);
        }

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

        // Destroy the current order instance
        if (currentOrderInstance != null)
        {
            Destroy(currentOrderInstance);
            currentOrderInstance = null; // Clear the reference after destroying
        }

        // Reset the processing flag
        isProcessing = false;
    }
}
