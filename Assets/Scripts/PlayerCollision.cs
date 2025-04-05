using UnityEngine;
using ludum;

public class PlayerCollision : MonoBehaviour
{
    bool isDead = false;
    public GameObject player;

    void Start()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with" + collision.gameObject.tag);

        if ((collision.gameObject.CompareTag("Terrain") ||
            collision.gameObject.CompareTag("Enemy")) && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Time.timeScale = 0f;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        Main playerMovement = GetComponent<Main>();
        if (playerMovement != null) playerMovement.SetDead();

        Debug.Log("Player died!");
    }
}