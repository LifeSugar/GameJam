using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;

    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}