using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public float pourThreshold = 5f;  // ��б�Ƕ���ֵ
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    private void Update()
    {
        // ��ȡ z ���ϵ���ת�Ƕ�
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        // ��ӡ z ����ת�Ƕ��Խ��е���
        Debug.Log("Current zRotation: " + zRotation);

        // �����ת�Ƕ��Ƿ񳬹���ֵ
        bool pourCheck = Mathf.Abs(zRotation) > pourThreshold;
        Debug.Log("PourCheck: " + pourCheck);

        // �� pour ״̬�仯ʱ����ʼ�������Һ��
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
