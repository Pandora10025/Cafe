using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkController : MonoBehaviour
{
    public GameObject milkObject; 
    public SpriteRenderer pitcherSpriteRenderer; 
    public Sprite pitcherWithMilkSprite; 
    public float tiltAngle = 20f; 
    public float tiltSpeed = 2f; 
    private bool isTilted = false; 
    private Quaternion originalRotation; 

    void Start()
    {
        originalRotation = milkObject.transform.rotation; 
    }

    void Update()
    {
        // tilt milk
        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit && hit.collider != null && hit.collider.gameObject == milkObject)
            {
                if (!isTilted)
                {
                    
                    StartCoroutine(TiltMilk(milkObject.transform.rotation, Quaternion.Euler(0, 0, -tiltAngle)));
                }
                else
                {
                    
                    StartCoroutine(TiltMilk(milkObject.transform.rotation, originalRotation));
                    pitcherSpriteRenderer.sprite = pitcherWithMilkSprite; 
                }

                isTilted = !isTilted; 
            }
        }
    }

  
    private IEnumerator TiltMilk(Quaternion startRotation, Quaternion targetRotation)
    {
        float elapsedTime = 0f;

        while (elapsedTime < tiltSpeed)
        {
            milkObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / tiltSpeed);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

       
        milkObject.transform.rotation = targetRotation;
    }
}
