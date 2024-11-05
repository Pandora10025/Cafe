using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public float pourThreshold = 5f;  // 倾斜角度阈值
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    private void Update()
    {
        // 获取 z 轴上的旋转角度
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        // 打印 z 轴旋转角度以进行调试
        Debug.Log("Current zRotation: " + zRotation);

        // 检查旋转角度是否超过阈值
        bool pourCheck = Mathf.Abs(zRotation) > pourThreshold;
        Debug.Log("PourCheck: " + pourCheck);

        // 当 pour 状态变化时，开始或结束倒液体
        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring)
            {
                Debug.Log("Calling StartPour");
                StartPour();
            }
            else
            {
                Debug.Log("Calling EndPour");
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
