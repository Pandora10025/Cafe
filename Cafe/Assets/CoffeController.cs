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
        // 打印当前 pitcher 的 sprite 名称
        Debug.Log("Current pitcher sprite: " + pitcher.GetComponent<SpriteRenderer>().sprite.name);

        Debug.Log("Making Cappuccino");

        
        if (other.gameObject.CompareTag("Cup") && pitcher.GetComponent<SpriteRenderer>().sprite.name == "pitcher wiz foam")
        {
            MakeCappuccino(other.gameObject);  // 制作 Cappuccino
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
