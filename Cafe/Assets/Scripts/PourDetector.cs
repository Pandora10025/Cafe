using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public float pourThreshold = 10f;  
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    private void Update()
    {
        // rotation on z axis
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        // zRotation angle
       

        // if z rotation is bigger than pourthreshold
        bool pourCheck = Mathf.Abs(zRotation) > pourThreshold;
       

        
        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring)
            {
               
                StartPour();
            }
            else
            {
               
                EndPour();
            }
        }
    }


    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        currentStream.End();
        currentStream = null;
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}