using System.Collections;
using UnityEngine;

public class HolderController : MonoBehaviour
{
    public Animator holderAnimator;  // Animator 
    public GameObject holder;  // Holder object
    public Transform holderPoint;  // Holder position
    

    private bool isMovingToHolderPoint = false;  // holder moving?

    void Update()
    {
        // click holder
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit && hit.collider != null && hit.collider.gameObject == holder && !isMovingToHolderPoint)
            {
                // move to the point
                StartCoroutine(MoveHolderToPoint());
            }
        }
    }

    private IEnumerator MoveHolderToPoint()
    {
        isMovingToHolderPoint = true;

        
        float moveSpeed = 5f;  
        while (Vector2.Distance(holder.transform.position, holderPoint.position) > 0.01f)
        {
            holder.transform.position = Vector2.MoveTowards(holder.transform.position, holderPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        
        holder.transform.position = holderPoint.position;
       

        
        if (holderAnimator != null)
        {
            holderAnimator.SetTrigger("holderMove");
        }
       

        isMovingToHolderPoint = false;
    }
}
