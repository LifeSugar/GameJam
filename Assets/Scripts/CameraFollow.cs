using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;    // 玩家对象的位置
    public Vector3 offset = new Vector3(0, 0, -10f); // 摄像机相对玩家的偏移值（-10f 是默认相机的偏移值）

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset; // 跟随玩家保持固定偏移
        }
    }
}