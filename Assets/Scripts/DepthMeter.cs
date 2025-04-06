using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthIndicator : MonoBehaviour
{
    [Header("绑定")]
    [SerializeField] Transform player;          // 玩家对象
    [SerializeField] RectTransform needle;      // 指针的RectTransform


    [Header("参数")]
    [SerializeField] float maxDepth = 100f;     // 最大深度
    [SerializeField] float minNeedleY = -200f;  // 指针最低位置
    [SerializeField] float maxNeedleY = 200f;   // 指针最高位置
    [SerializeField] float dangerThreshold = 80f;// 危险阈值
    [SerializeField] float smoothSpeed = 5f;    // 移动平滑度

    private float initialY;
    private Vector2 needleStartPos;

    void Start()
    {
        initialY = player.position.y;
        needleStartPos = needle.anchoredPosition;


    }

    void Update()
    {
        UpdateNeedlePosition();
       
    }

    void UpdateNeedlePosition()
    {
        // 计算当前深度（Y越小深度值越大）
        float currentDepth = Mathf.Clamp(initialY - player.position.y, 0, maxDepth);

        // 计算插值比例
        float t = currentDepth / maxDepth;

        // 计算目标Y位置
        float targetY = Mathf.Lerp(maxNeedleY, minNeedleY, t);

        // 平滑移动
        Vector2 newPos = new Vector2(
            needle.anchoredPosition.x,
            Mathf.Lerp(needle.anchoredPosition.y, targetY, smoothSpeed * Time.deltaTime)
        );

        needle.anchoredPosition = newPos;
    }

   
}
