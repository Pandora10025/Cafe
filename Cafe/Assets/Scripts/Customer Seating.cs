using UnityEngine;
using System.Collections;

public class CustomerSeatingManager : MonoBehaviour
{
    public GameObject foxSeatingPrefab;    
    public GameObject raccoonSeatingPrefab;   
    public GameObject dogSeatingPrefab;       

    public Transform foxPosition;             
    public Transform raccoonPosition;       
    public Transform dogPosition;           

    public void CreateSeating(ScriptOb_Customer customer)
    {
        GameObject seatingPrefab = null;
        Transform seatingPosition = null;

        // check the name to determine the type of the customer
        string customerName = customer.name;
        if (customerName == "Customer_Fox")
        {
            seatingPrefab = foxSeatingPrefab;
            seatingPosition = foxPosition;
        }
        else if (customerName == "Customer_Raccoon")
        {
            seatingPrefab = raccoonSeatingPrefab;
            seatingPosition = raccoonPosition;
        }
        else if (customerName == "Customer_Dog")
        {
            seatingPrefab = dogSeatingPrefab;
            seatingPosition = dogPosition;
        }

        // instantiate seating
        if (seatingPrefab != null && seatingPosition != null)
        {
            // if the customer is seated
            if (seatingPosition.childCount == 0)
            {
                GameObject seatingInstance = Instantiate(seatingPrefab, seatingPosition.position, Quaternion.identity, seatingPosition);
                StartCoroutine(DestroyAfterTime(seatingInstance, 20.0f)); 
            }
            else
            {
                Debug.Log("Position already occupied by another customer seating.");
            }
        }
    }

    private IEnumerator DestroyAfterTime(GameObject seating, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (seating != null)
        {
            Destroy(seating);
        }
    }
}

