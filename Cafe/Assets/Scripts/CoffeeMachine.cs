using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Transform coffeeSnapPoint;
    public Transform cupBottom;
    public Transform cup;
    public Animator cupAnimator; // Animator for the cup to play coffee-making animation
    public bool isProducing = false;
    public bool isCupSnapped = false;

    private void Update()
    {
        // Start coffee production when cup is snapped and left mouse button is pressed
        if (Input.GetMouseButtonDown(0) && isCupSnapped)
        {
            StartCoffeeProduction();
            Debug.Log("Coffee making");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTriggerStay2D triggered with: " + other.name);
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = true;
            Debug.Log("Cup is ready");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Not Collide");
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = false;
            Debug.Log("Cup is not ready");
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;

            // Play coffee-making animation
            if (cupAnimator != null)
            {
                cupAnimator.SetTrigger("StartCoffeeMaking");
            }

            Debug.Log("Coffee making started");
        }
    }
}
