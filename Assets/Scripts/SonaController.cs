using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SonaController : MonoBehaviour
{
    public Transform sonaHitPoint;
    public Material outlineMaterial;
    public float range = 0.2f;
    public float softness = 0.1f;

    public static SonaController instance;

    private bool isSonaActive = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSonaActive = !isSonaActive;  // 状态反转一次
            if (isSonaActive)
            {
                SetSonaHitPoint(sonaHitPoint.position);
            }
            else
            {
                outlineMaterial.SetFloat("_MaskRadius", 0f);
            }
        }

        if (isSonaActive)
        {
            Vector3 screenPos = Camera.main.WorldToViewportPoint(sonaHitPoint.position);
            outlineMaterial.SetVector("_MaskCenter", new Vector4(screenPos.x, screenPos.y, 0, 0));
            outlineMaterial.SetFloat("_MaskRadius", range);
            outlineMaterial.SetFloat("_MaskSoftness", softness);
        }
    }

    void SetSonaHitPoint(Vector3 hitPoint)
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(hitPoint);
        outlineMaterial.SetVector("_MaskCenter", new Vector4(screenPos.x, screenPos.y, 0, 0));
    }
}