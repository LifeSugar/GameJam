using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

[ExecuteAlways]
public class SonaController : MonoBehaviour
{
    public Transform sonaHitPoint;
    public Material outlineMaterial;
    [FormerlySerializedAs("range")] public float currentRange = 0.2f;
    [FormerlySerializedAs("softness")] public float currentSoftness = 0.1f;

    [Header("声纳属性")]
    [Range(0,1)] public float Range;
    [Range(0,1)] public float Softness;
    public float fadeTime = 1f;

    public static SonaController instance;
    
    [Header("发射声纳")]
    public bool EmitSona = false;
    public float EmitSpeed = 0.2f;
    public bool emitting;
    [SerializeField]private Vector2 debugCurrentPos;
    
    [Header("点击声纳")]
    public bool CursorSona = false;

    [Header("玩家位置")]
    public Transform player;

    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        outlineMaterial.SetFloat("_Range", 0f);
        outlineMaterial.SetFloat("_Softness", 1f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!EmitSona && !CursorSona)
        {
            HandleTest();
        }
        else if (EmitSona && !CursorSona)
        {
            HandleEmitSona();
        }

    }

    void HandleTest()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(sonaHitPoint.position);
        outlineMaterial.SetVector("_MaskCenter", new Vector4(screenPos.x, screenPos.y, 0, 0));
        outlineMaterial.SetFloat("_MaskRadius", Range);
        outlineMaterial.SetFloat("_MaskSoftness", Softness);
    }

    private Tweener moveTween;     
    [SerializeField]private Vector3 currentTweenPos;
    void HandleEmitSona()
    {
        // 检测到鼠标左键按下
        if (Input.GetMouseButtonDown(0) && !emitting)
        {
            // 将鼠标屏幕坐标转换为世界坐标（2D）
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 将 Z 坐标与 player 保持一致
            mousePos.z = player.position.z;

            // 计算方向（归一化）
            Vector2 direction = (mousePos - player.position).normalized;

            // 获取 "terrain" 层的 LayerMask
            int layerMask = 1 << LayerMask.NameToLayer("terrain");

            // 从玩家位置向鼠标点击方向发射射线
            RaycastHit2D hit = Physics2D.Raycast(
                player.position,   // 射线起点
                direction,         // 射线方向
                Mathf.Infinity,    // 最大射线距离
                layerMask          // 只检测 "terrain" 层
            );

            // 如果击中到 "terrain" 层的碰撞体
            if (hit.collider != null)
            {
                // Debug.Log("Hit terrain at: " + hit.point);
                outlineMaterial.SetFloat("_MaskRadius", Range);
                outlineMaterial.SetFloat("_MaskSoftness", Softness);
                currentRange = Range;
                currentSoftness = Softness;

                // 终点（射线击中的世界坐标）
                Vector3 targetPos = hit.point;
                targetPos.z = player.position.z; // 与玩家 z 对齐或你所需 z

                // 杀死之前没完成的 Tween（防止连点造成并行冲突）
                if (moveTween != null && moveTween.IsActive())
                {
                    moveTween.Kill();
                }

                // 记录插值的起点
                currentTweenPos = player.position;

                // 计算本次移动需要的时长：距离 / 速度
                float distance = Vector2.Distance(currentTweenPos, targetPos);
                float duration = distance / EmitSpeed;

                // 使用 DOTween 做插值：让 currentTweenPos 在若干秒内从起点移动到终点,并伴随衰减
                SetSonaFade(targetPos);
                moveTween = DOTween.To(
                        () => currentTweenPos,
                        x => currentTweenPos = x,
                        targetPos,
                        duration
                    )
                    .SetEase(Ease.Linear)     // 匀速插值
                    .OnUpdate(() =>
                    {
                        // emitting = true;
                        // 插值进行时，将 currentTweenPos 设置到 outlineMaterial
                        var currentScreenPos = Camera.main.WorldToViewportPoint(currentTweenPos);
                        outlineMaterial.SetVector("_MaskCenter",
                            new Vector4(currentScreenPos.x, currentScreenPos.y, 0, 0));
                    })
                    .OnComplete(() =>
                    {
                        Debug.Log("Sona animation complete!");
                        // emitting = false;
                        
                    });
            }
        }
        
    }

    
    //这里默认的range softness是0.4，0.5
    void SetSonaFade(Vector3 targetPos)
    {
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Join(
            DOTween.To(
                () => this.currentSoftness,
                x =>
                {
                    this.currentSoftness = x;
                    outlineMaterial.SetFloat("_MaskSoftness", currentSoftness);
                }, 1f, this.fadeTime))
            .OnUpdate(() =>
            {
                emitting = true;
                var currentScreenPos = Camera.main.WorldToViewportPoint(targetPos);
                outlineMaterial.SetVector("_MaskCenter", 
                    new Vector4(currentScreenPos.x, currentScreenPos.y, 0, 0));
            });

        fadeSequence.Join(
            DOTween.To(
                () => this.currentRange,
                x =>
                {
                    this.currentRange = x;
                    outlineMaterial.SetFloat("_MaskRadius", currentRange);
                }, 0f, this.fadeTime
            )
        );
        
        

        fadeSequence.OnComplete(() =>
        {
            emitting = false;
            moveTween.Kill();
        });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(player.position, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position).normalized * 100f);

        Vector3 worldPos =
            new Vector3(currentTweenPos.x, currentTweenPos.y, 0);
        

        // 在 Scene 视图中显示一个黄色的圆
        Gizmos.color = Color.yellow;
        // 画一个线框球，半径可自行调整
        Gizmos.DrawWireSphere(worldPos, 2f);

    }
}