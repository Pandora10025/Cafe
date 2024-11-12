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
    public int currentCustomers;
    int maxCustomers;

    public AudioSource DoorBell;
    
   public void Start()
    {
        gameManager = GameManager.instance; 

        currentCustomers = gameManager.currentCustomers;

        maxCustomers = gameManager.maxCustomers;

        SpawnCustomers();
    }

    public int SpawnCustomers()
    {
        Debug.Log("Im in customer generator");
        if (currentCustomers < maxCustomers)
        {
            int tempIndex = Random.Range(0, customer.Count);

            GameObject customerTemp = Instantiate(customer_prefab);
            DoorBell.Play();

            ScriptOb_Customer_Controller tempController = customerTemp.GetComponent<ScriptOb_Customer_Controller>();

            while (tempIndex >= customer.Count)
            {
                tempIndex = Random.Range(0, customer.Count);
            }

            tempController.customer = customer[tempIndex];
            tempController.SetUp();

            currentCustomers++;

            return currentCustomers;
        }
        else
        {
            return currentCustomers;
        }

    }

    
}
