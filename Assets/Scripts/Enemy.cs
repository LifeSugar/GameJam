using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public bool isAlert;
    public float alertDistance;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Get player coordinates
        isAlert = false;
        speed = 5f;
        alertDistance = 3f;
    }

    void Update()
    {
        if (!isAlert)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            bool isAbovePlayer = player.position.y < transform.position.y;

            if (distance < alertDistance && isAbovePlayer) // Check if the player is below the enemy and in detection distance
            {
                isAlert = true;
            }
        }
        else
        {
            ChasePlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Check if the enemy has collided with the player
        {
            Debug.Log("You Died!");
        }

        if (other.gameObject.CompareTag("Enemy Blocked!"))
        {
            // Enemy blocked by terrain and self destroyed
            Destroy(gameObject);
        }
    }

    void ChasePlayer()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 playerPosition = player.position;

        transform.position = Vector2.MoveTowards(enemyPosition, playerPosition, speed * Time.deltaTime); // Enemy chasing the player
    }
}