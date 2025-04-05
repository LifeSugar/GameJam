using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class TilemapOutlineRenderer : MonoBehaviour
{
    void Start()
    {
        CompositeCollider2D composite = GetComponent<CompositeCollider2D>();
        LineRenderer lr = GetComponent<LineRenderer>();

        // 假设只取第一条 Path；如果有多个不连续区域，可遍历 i in [0..pathCount-1]
        int pathCount = composite.pathCount;
        for (int i = 0; i < pathCount; i++)
        {
            Vector2[] points = new Vector2[composite.GetPathPointCount(i)];
            int pointCount = composite.GetPath(i, points);

            // 如果只想画一条外轮廓，就要把所有 path 都合并。否则可对每条 path 生成一个 LineRenderer
            // 这里演示单条 path 用一个 LR:
            lr.positionCount = pointCount;
            for (int p = 0; p < pointCount; p++)
            {
                lr.SetPosition(p, points[p]);
            }
            // 视需要决定是否闭合：LineRenderer.loop = true;
        }
    }
}