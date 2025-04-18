using UnityEngine;
using System.Collections;

public class ExplodingEnemy : MonoBehaviour
{
    public float triggerRadius = 5f;          // 玩家接近触发范围
    public float explosionRadius = 3f;        // 爆炸范围
    public float timeToExplode = 2f;          // 从变色到爆炸的延迟时间
    public GameObject explosionEffect;        // 可选：爆炸特效prefab
    public Animator anim;
    public bool isTriggered = false;

    void Start()
    {

    }

    void Update()
    {
        // 如果还未触发，持续监测玩家与自己的距离
        if (!isTriggered)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= triggerRadius)
                {
                    StartCoroutine(StartExplosionSequence());
                    anim.SetBool("isTriggered", true);
                }
            }
        }
    }

    IEnumerator StartExplosionSequence()
    {
        isTriggered = true;


        // 等待指定延迟后爆炸
        yield return new WaitForSeconds(timeToExplode);

        Explode();
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                ludum.Main playerScript = hit.GetComponent<ludum.Main>();
                if (playerScript != null)
                {
                    playerScript.SetDead();
                    Time.timeScale = 0f;
                }
            }
        }
        Destroy(gameObject);
        anim.SetBool("isTriggered", false);
}

    // 可选：在编辑器中显示触发与爆炸范围，方便设计者调整
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}