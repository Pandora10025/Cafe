using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOb_Customer_Controller : MonoBehaviour
{
  // grab customer scriptable object
    public ScriptOb_Customer customer;

    void Start()
    {
        
    }

    public void SetUp()
    {
        // grab sprite renderer of this prefab instance
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // set the sprite to the assigned customer scriptable object from customer generator 
        spriteRenderer.sprite = customer.sprite;

    }
}
