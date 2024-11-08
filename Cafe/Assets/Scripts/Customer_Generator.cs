using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Customer_Generator : MonoBehaviour
{
   GameManager gameManager;

    // spawning prefabs
    public GameObject customer_prefab;
   

    // customer scriptable objects list
    public List<ScriptOb_Customer> customer = new List<ScriptOb_Customer>();


    // keeping track of how many customers have spawned
    int currentCustomers;
    int maxCustomers;

    
    void Start()
    {
        gameManager = GameManager.instance; 

        currentCustomers = 0;

        maxCustomers = 1;

        SpawnCustomers();
    }

    public void SpawnCustomers()
    {
        if (currentCustomers < maxCustomers)
        {
            int tempIndex = Random.Range(0, customer.Count);

            GameObject customerTemp = Instantiate(customer_prefab);

            ScriptOb_Customer_Controller tempController = customerTemp.GetComponent<ScriptOb_Customer_Controller>();

            while (tempIndex >= customer.Count)
            {
                tempIndex = Random.Range(0, customer.Count);
            }

            tempController.customer = customer[tempIndex];
            tempController.SetUp();

            currentCustomers++;
        }

    }

    
}
