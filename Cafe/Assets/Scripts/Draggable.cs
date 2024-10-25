using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public delegate void DragEndedDelegate(Draggable draggableObject);
    public DragEndedDelegate dragEndedCallback; 

    private bool isDragged = false;
    private Vector3 offset; 
    private Camera mainCamera; 

    private void Start()
    {
        mainCamera = Camera.main; 
    }

    private void OnMouseDown()
    {
        
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mouseWorldPosition;
        isDragged = true; 
    }

    private void OnMouseDrag()
    {
        if (isDragged)
        {
           
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x + offset.x, mouseWorldPosition.y + offset.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        
        isDragged = false;
        dragEndedCallback?.Invoke(this); 
    }
}
