using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public Transform coffeeSnapPoint; 
    public Transform cupBottom;
    public Transform cup;
    public Animator cupAnimator; 
    public Animator coffeeMachineAnimator; // Animator£¨
    public Transform holder; // Reference to the holder object
    public Transform holderPoint; // Reference to the holder's target point
    public float holderSnapThreshold = 0.1f; 
    public bool isProducing = false;
    public bool isCupSnapped = false;

    private void Update()
    {
        // if mouse on coffee machine
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(0))
        {
            // coffee makable
            Debug.Log("Mouse clicked on CoffeeMachine");
            if (isCupSnapped)
            {
                Debug.Log("Cup is snapped in place.");
            }
            if (IsHolderInPosition())
            {
                Debug.Log("Holder is in position.");
            }

            if (isCupSnapped && IsHolderInPosition())
            {
                StartCoffeeProduction();
                Debug.Log("Coffee making");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = true; 
            Debug.Log("Cup snapped into position");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cup"))
        {
            isCupSnapped = false; // cup leave the coffeemachine£¨…Ë÷√ isCupSnapped Œ™ false
            Debug.Log("Cup unsnapped from position");

            // set coffee machine animation to idle
            if (coffeeMachineAnimator != null)
            {
                coffeeMachineAnimator.SetTrigger("Idle");
                Debug.Log("CoffeeMachine returned to idle animation");
            }
        }
    }

    private void StartCoffeeProduction()
    {
        if (!isProducing)
        {
            isProducing = true;

            // coffee machine animation
            if (coffeeMachineAnimator != null)
            {
                coffeeMachineAnimator.SetTrigger("StartCoffeeMaking");
                Debug.Log("CoffeeMachine started coffee-making animation");
            }

            // play making coffee animation
            if (cupAnimator != null)
            {
                cupAnimator.SetTrigger("StartCoffeeMaking");
                Debug.Log("Cup started coffee-making animation");
            }

            Debug.Log("Coffee making started");
        }
    }

    // if holder is on the right place
    private bool IsHolderInPosition()
    {
        if (holder == null || holderPoint == null)
        {
            return false;
        }

        float distance = Vector3.Distance(holder.position, holderPoint.position);
        return distance <= holderSnapThreshold;
    }
}
