using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOb_Customer_Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public ScriptOb_Customer customer;
    void Start()
    {
        
    }

    public void SetUp()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = customer.sprite;
    }
}
