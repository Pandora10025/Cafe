using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MilkFoamMaker : MonoBehaviour
{
    public GameObject pitcher;  
    public GameObject steamWand;        
    public GameObject pointer;         
    public GameObject progressBar;      
    public float pointerSpeed = 1.0f;   
    public float perfectStartX = 15.9f;  
    public float perfectEndX = 57.3f;    
    public float progressEndX = 116.3f;   

    public Sprite pitcherWithFoamSprite;  
    public Sprite pitcherWithMilkSprite;

    private RectTransform pointerRectTransform;  
    private RectTransform progressBarRectTransform;  
    private bool isFoaming = false; 

    private void Start()
    {
        pointerRectTransform = pointer.GetComponent<RectTransform>();
        progressBarRectTransform = progressBar.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isFoaming)
        {
           
            pointerRectTransform.anchoredPosition += new Vector2(pointerSpeed * Time.deltaTime, 0);

            float pointerX = pointerRectTransform.anchoredPosition.x;

            //Perfect?
            if (pointerX >= perfectStartX && pointerX <= perfectEndX)
            {
                Debug.Log("Perfect!");
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;
            }

            
            if (pointerX >= progressEndX)
            {
                StopFoamingProcess(); 
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;  
                Debug.Log("Finished foaming!");
            }

            
            if (Input.GetMouseButtonDown(0))
            {
                StopFoamingProcess();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide with pitcher");

        
        if (other.gameObject == pitcher && pitcher.GetComponent<SpriteRenderer>().sprite == pitcherWithMilkSprite)
        {
            StartFoamingProcess();  
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.gameObject == pitcher)
        {
            StopFoamingProcess();
        }
    }

    private void StartFoamingProcess()
    {
        if (!isFoaming)
        {
            isFoaming = true;
            pointer.SetActive(true);  // ¼¤»î pointer
            progressBar.SetActive(true);  // ¼¤»î progress bar
            pointerRectTransform.anchoredPosition = new Vector2(0, pointerRectTransform.anchoredPosition.y);  // ³õÊ¼»¯ pointer Î»ÖÃ
        }
    }

    private void StopFoamingProcess()
    {
        isFoaming = false;
        pointer.SetActive(false);  // Òþ²Ø pointer
        progressBar.SetActive(false);  // Òþ²Ø progress bar
    }
}
