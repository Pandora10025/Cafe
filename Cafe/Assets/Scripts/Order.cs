using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    public GameObject order_prefab;  // Customer Order speech bubble prefab
    public GameObject speechBubblePrefab;  // Prefab for the speech bubble
    private GameObject currentOrderParentInstance;  // Parent GameObject to hold both order and speech bubble

    [SerializeField]
    public List<ScriptOb_Drinks> drinks = new List<ScriptOb_Drinks>(); // Drinks scriptable object list

         
    public GameObject customer;          // Customer Object

    public Sprite NcupSprite;

    public CoffeeMachine coffeeMachine;

     
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
        cupAnimator = FindAnyObjectByType<Animator>();

        // Store the starting position of the customer
        customerStartPos = customer.transform.position;

        

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
            if (cupSpriteRenderer != null && currentOrderParentInstance != null)
            {
                // Get the sprite on the current order instance
                SpriteRenderer orderSpriteRenderer = currentOrderParentInstance.GetComponentInChildren<SpriteRenderer>();

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

        // Create a parent GameObject to hold both order and speech bubble
        currentOrderParentInstance = new GameObject("OrderWithSpeechBubble");

        // Instantiate the order prefab and assign it as a child of the parent GameObject
        GameObject orderInstance = Instantiate(order_prefab, new Vector3(pos_x + 3.4f, pos_y + 2.5f, pos_z), Quaternion.identity, currentOrderParentInstance.transform);
        currentOrderParentInstance = orderInstance; // Update currentOrderParentInstance with newly created GameObject instance.

        // Instantiate the speech bubble prefab and assign it as a child of the parent GameObject
        GameObject speechBubbleInstance = Instantiate(speechBubblePrefab, new Vector3(-19, 1, 0), Quaternion.identity, currentOrderParentInstance.transform);

        // Set up the current order instance with the drink information
        ScriptOb_Drinks_Controller tempController = orderInstance.GetComponent<ScriptOb_Drinks_Controller>();

        if (tempController != null && tempIndex < drinks.Count)
        {
            tempController.drink = drinks[tempIndex];
            tempController.SetUp();
        }
        else
        {
            Debug.LogError("Error setting up the order instance.");
        }

        return currentOrderParentInstance;
    }

    private IEnumerator ProcessOrder(SpriteRenderer cupSpriteRenderer)
    {
        isProcessing = true;

        // Step 1: Deactivate the current order
        if (currentOrderParentInstance != null)
        {
            currentOrderParentInstance.SetActive(false);
        }

        // Step 2: Change cup animator back to idle
        if (cupAnimator != null)
        {
            cupAnimator.enabled = true; // Enable the cup's animator
            cupAnimator.SetTrigger("BackToIdle"); // Trigger the Idle animation
            Debug.Log("Cup animator enabled and BackToIdle trigger set");
        }
        else
        {
            Debug.Log("Cupanimator is null");
        }

        CoffeeMachine coffeeMachineScript = coffeeMachine.GetComponent<CoffeeMachine>();
        coffeeMachineScript.isProducing = false;   // Reset producing state
        coffeeMachineScript.isCupSnapped = false;  // Allow the cup to be snapped again

        // Step 3: Move the customer to the left
        Vector3 targetPosition = customerStartPos + Vector3.left * customerMoveDistance;
        while (Vector3.Distance(customer.transform.position, targetPosition) > 0.1f)
        {
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPosition, customerMoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        if (customer != null)
        {
            // Step 4: Get customer type before destroying
            ScriptOb_Customer_Controller customerController = customer.GetComponent<ScriptOb_Customer_Controller>();
            if (customerController != null && customerController.customer != null)
            {
                ScriptOb_Customer customerScriptableObject = customerController.customer;

                // Step 5: Destroy the current customer instance
                Destroy(customer);
                customer = null; // Clear the reference after destroying

                // Step 6: Reduce the customer count and spawn a new customer
                Customer_Generator customerGenerator = FindObjectOfType<Customer_Generator>();
                if (customerGenerator != null)
                {
                    customerGenerator.currentCustomers--;  // Reduce the current customer count to allow new spawn
                    customerGenerator.SpawnCustomers();    // Spawn new customer
                }

                // Step 7: Generate seating for the customer that was served
                CustomerSeatingManager seatingManager = FindObjectOfType<CustomerSeatingManager>();
                if (seatingManager != null)
                {
                    seatingManager.CreateSeating(customerScriptableObject); // Pass the Scriptable Object to CreateSeating
                }
            }
        }

        // Step 8: Destroy the current order instance along with its speech bubble
        if (currentOrderParentInstance != null)
        {
            Destroy(currentOrderParentInstance);
            currentOrderParentInstance = null; // Clear the reference after destroying
        }

        // Step 9: Reset the processing flag
        isProcessing = false;
    }


}
