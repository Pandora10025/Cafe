using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject coffeeSprite; 
    public Transform coffeeSnapPoint; 
    public Transform cupBottom; 
    public float stretchDuration = 0.1f; 
    private bool isProducing = false; 
    private bool isCupSnapped = false; 

    private void Update()
    {
        if (isProducing)
        {
            coffeeSprite.transform.position = cupBottom.position; // coffee with the cup
        }

        
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
            coffeeSprite.SetActive(true); 

           
            coffeeSprite.transform.position = cupBottom.position;

            StartCoroutine(ProduceCoffee());
            Debug.Log("Coffee making");
        }
    }

    private IEnumerator ProduceCoffee()
    {
        Vector3 originalScale = coffeeSprite.transform.localScale;

        // more and more coffee
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y + 5.0f, originalScale.z);

        float stretchSpeed = 10.0f; 
        float elapsedTime = 0f;

        while (elapsedTime < stretchDuration)
        {
            
            coffeeSprite.transform.localScale = Vector3.Lerp(coffeeSprite.transform.localScale, targetScale, stretchSpeed * Time.deltaTime);

            
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

       
        coffeeSprite.transform.localScale = targetScale;

        // reset coffee making, move cup
        isCupSnapped = false;

        isProducing = false;
    }



}
