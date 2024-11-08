using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOb_Drinks_Controller : MonoBehaviour
{
    public ScriptOb_Drinks drink;
    void Start()
    {
        
    }

    public void SetUp()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = drink.sprite;
    }
}
