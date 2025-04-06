using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthIndicator : MonoBehaviour
{
    [Header("��")]
    [SerializeField] Transform player;          // ��Ҷ���
    [SerializeField] RectTransform needle;      // ָ���RectTransform


    [Header("����")]
    [SerializeField] float maxDepth = 100f;     // ������
    [SerializeField] float minNeedleY = -200f;  // ָ�����λ��
    [SerializeField] float maxNeedleY = 200f;   // ָ�����λ��
    [SerializeField] float dangerThreshold = 80f;// Σ����ֵ
    [SerializeField] float smoothSpeed = 5f;    // �ƶ�ƽ����

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
        // ���㵱ǰ��ȣ�YԽС���ֵԽ��
        float currentDepth = Mathf.Clamp(initialY - player.position.y, 0, maxDepth);

        // �����ֵ����
        float t = currentDepth / maxDepth;

        // ����Ŀ��Yλ��
        float targetY = Mathf.Lerp(maxNeedleY, minNeedleY, t);

        // ƽ���ƶ�
        Vector2 newPos = new Vector2(
            needle.anchoredPosition.x,
            Mathf.Lerp(needle.anchoredPosition.y, targetY, smoothSpeed * Time.deltaTime)
        );

        needle.anchoredPosition = newPos;
    }

   
}
