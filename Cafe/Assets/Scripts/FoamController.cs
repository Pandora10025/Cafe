using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MilkFoamMaker : MonoBehaviour
{
    public GameObject pitcher;  // pitcher with milk 的对象
    public GameObject steamWand;        // steam wand 的对象
    public GameObject pointer;          // pointer UI
    public GameObject progressBar;      // progress bar UI 对象
    public float pointerSpeed = 1.0f;   // pointer 移动速度
    public float perfectStartX = 15.9f;  // 完美区间的开始 x 坐标
    public float perfectEndX = 57.3f;    // 完美区间的结束 x 坐标
    public float progressEndX = 116.3f;   // progress bar 结束位置的 x 坐标

    public Sprite pitcherWithFoamSprite;  // 有奶泡的 pitcher sprite
    public Sprite pitcherWithMilkSprite;

    private RectTransform pointerRectTransform;  // pointer 的 RectTransform
    private RectTransform progressBarRectTransform;  // progress bar 的 RectTransform
    private bool isFoaming = false;  // 是否正在制作奶泡

    private void Start()
    {
        pointerRectTransform = pointer.GetComponent<RectTransform>();
        progressBarRectTransform = progressBar.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isFoaming)
        {
            // Pointer 移动逻辑
            pointerRectTransform.anchoredPosition += new Vector2(pointerSpeed * Time.deltaTime, 0);

            float pointerX = pointerRectTransform.anchoredPosition.x;

            // 检查是否在完美区间
            if (pointerX >= perfectStartX && pointerX <= perfectEndX)
            {
                Debug.Log("Perfect!");
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;
            }

            // 如果 pointer 移动到 progressBar 末端
            if (pointerX >= progressEndX)
            {
                StopFoamingProcess();  // 停止奶泡制作
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;  // 更改为有奶泡的 pitcher
                Debug.Log("Finished foaming!");
            }

            // 检测玩家是否手动移开 pitcher
            if (Input.GetMouseButtonDown(0))
            {
                StopFoamingProcess();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide with pitcher");

        // 检查 pitcher 是否与 steam wand 碰撞，且 sprite 是 pitcher wiz milk
        if (other.gameObject == pitcher && pitcher.GetComponent<SpriteRenderer>().sprite == pitcherWithMilkSprite)
        {
            StartFoamingProcess();  // 开始奶泡制作
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 当 pitcher 离开 steam wand 时，停止奶泡制作
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
            pointer.SetActive(true);  // 激活 pointer
            progressBar.SetActive(true);  // 激活 progress bar
            pointerRectTransform.anchoredPosition = new Vector2(0, pointerRectTransform.anchoredPosition.y);  // 初始化 pointer 位置
        }
    }

    private void StopFoamingProcess()
    {
        isFoaming = false;
        pointer.SetActive(false);  // 隐藏 pointer
        progressBar.SetActive(false);  // 隐藏 progress bar
    }
}
