using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    public GameObject pitcher;  // Pitcher object
    public GameObject coffeeObject;  // Coffee object inside the cup
    public Sprite cappuccinoSprite;  // Sprite for cappuccino
    public SpriteRenderer cupSpriteRenderer;  // SpriteRenderer for the cup to change the sprite

    private void OnTriggerEnter2D(Collider2D other)
    {
       
      
       
        Debug.Log(other.gameObject.name);

      
        if (other.gameObject.CompareTag("Cup") && pitcher.GetComponent<SpriteRenderer>().sprite.name == "pitcher wiz foam 1")
        {
            
            Transform coffeeInCup = other.transform.Find("Coffee");

            foreach (Transform child in other.gameObject.transform)
            {
                Debug.Log("Child object: " + child.name);
            }

            if (coffeeInCup != null && coffeeInCup.gameObject.activeSelf)
            {
                MakeCappuccino(other.gameObject);  
            }
            else
            {
                Debug.Log("No coffee in the cup, cannot make Cappuccino.");
            }
        }
    }



    private void MakeCappuccino(GameObject cup)
    {
        Debug.Log("Cappuccino is made!");

        
        SpriteRenderer cupRenderer = cup.GetComponent<SpriteRenderer>();

        if (cupRenderer != null)
        {
           
            cupRenderer.sprite = cappuccinoSprite;

            Debug.Log("Cup sprite changed to: " + cupRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("Cup does not have a SpriteRenderer component.");
        }

        
        coffeeObject.SetActive(false);
    }

}
