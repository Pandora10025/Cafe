using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    
    public GameObject order;   // Order Object
    string orderName; // order name 
    public GameObject order_prefab; //Customer Order speech bubble
    [SerializeField]
    public List<ScriptOb_Drinks> drinks = new List<ScriptOb_Drinks>(); // drinks scriptable object list

    public GameManager gameManager;  // Game manager is need for later

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
        // Grabbing the game manager instance
        gameManager = GameManager.instance;

        // Store the starting position of the customer
        customerStartPos = customer.transform.position;

        // Ensure Thank You is deactivated at the start
        thankYou.SetActive(false);

        // find cup in the scene
        GameObject cup = GameObject.FindGameObjectWithTag("Cup");
        
        // assing animator for cup to cup animator
        cupAnimator = cup.GetComponent<Animator>();

        pos_x = this.transform.position.x;
        pos_y = this.transform.position.y;
        pos_z = this.transform.position.z;

        GenerateOrder();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        orderName = order.GetComponent<Sprite>().name;

        // Check if the colliding object is the cup with the cappuccino sprite
        SpriteRenderer cupSpriteRenderer = other.GetComponent<SpriteRenderer>();
        if (other.gameObject.CompareTag("Cup") && cupSpriteRenderer != null && cupSpriteRenderer.sprite.name == orderName)
        {
            if (!isProcessing)
            {
                StartCoroutine(ProcessOrder(cupSpriteRenderer));
            }
        }
    }

    public GameObject GenerateOrder()
    {

        int tempIndex = Random.Range(0, drinks.Count);
        Debug.Log(tempIndex);

        GameObject order = Instantiate(order_prefab, new Vector3(pos_x + 6, pos_y + 3, pos_z), Quaternion.identity);

        ScriptOb_Drinks_Controller tempController = order.GetComponent<ScriptOb_Drinks_Controller>();

        while (tempIndex >= drinks.Count)
        {
            tempIndex = Random.Range(0, drinks.Count);
        }

        tempController.drink = drinks[tempIndex];
        tempController.SetUp();

        return order;
    }

    private IEnumerator ProcessOrder(SpriteRenderer cupSpriteRenderer)
    {
        isProcessing = true;

        // Step 1: Deactivate the current order
        order.SetActive(false);

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

        // Tell Customer Generator Script that there's no customer any more 
        gameManager.currentCustomers = gameManager.currentCustomers--;

        // Reset the processing flag
        isProcessing = false;

        //Destroy this gameobject 

        Destroy(gameObject);
    }

    private IEnumerator CustomerEnter()
    {

        // Step 7: Wait for a delay before the customer returns
        yield return new WaitForSeconds(customerReturnDelay);

        // Step 8: Move the customer back to the starting position
        while (Vector3.Distance(customer.transform.position, customerStartPos) > 0.1f)
        {
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, customerStartPos, customerMoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}
