using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public enum GameState{
    Start,
    Quit,
    Restart,
    Customer,
    Serving
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    Customer_Generator _generator;

    public int currentCustomers = 0;
    public int maxCustomers = 1;

    GameState state;

    // spawning prefabs
    public GameObject customer_prefab;


    // customer scriptable objects list
    public List<ScriptOb_Customer> customer = new List<ScriptOb_Customer>();

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
       _generator = GetComponent<Customer_Generator>();

        state = GameState.Customer;

        Game(state);
    }

    private void FixedUpdate()
    {
        if (currentCustomers < maxCustomers)
        {
            state = GameState.Customer;
        }
        else
        {
            state = GameState.Serving;
        }
    }

    public void Game(GameState state)
    {
        switch(state)
        {
            case GameState.Start:
                // load start screen

                break;
            case GameState.Quit:
                Application.Quit();
                break;
            case GameState.Restart:
               // load start screen 

                break;
            case GameState.Customer:
                Debug.Log("I'm in customer state;");


                _generator.Start();
                break;
            case GameState.Serving:
                Debug.Log("I'm in serving state");

                break;
            default:
                Debug.Log(state.ToString() + "could not be found.");
                state = GameState.Start;
                break;


        }
    }
    public void SwitchScene(string newScene)
    {
        Debug.Log("switch scene called " + newScene);
        SceneManager.LoadScene(newScene);
       
    }

}
