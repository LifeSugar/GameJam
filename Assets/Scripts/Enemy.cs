using UnityEngine;

namespace ludum {
    public class Enemy : MonoBehaviour
    {
        public float enemySpeed;
        public bool isAlert;
        public float alertDistance;
        private Transform player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Get player coordinates
            isAlert = false;
            enemySpeed = 5f;
            alertDistance = 7f;
        }

        void Update()
        {
            if (Time.timeScale == 0f) return;

            if (!isAlert)
            {
                float distance = Vector2.Distance(transform.position, player.position);
                bool isAbovePlayer = player.position.y < transform.position.y;
                isAlert = distance < alertDistance && isAbovePlayer;
            }
            else
            {
                ChasePlayer();
            }

           
        }

        void ChasePlayer()
        {
            Vector2 enemyPosition = transform.position;
            Vector2 playerPosition = player.position;

            transform.position = Vector2.MoveTowards(enemyPosition, playerPosition, enemySpeed * Time.deltaTime); // Enemy chasing the player
        }

        private void OnCollisionEnter2D(Collision2D collision)//碰到地形销毁
        {
            if (collision.gameObject.CompareTag("Terrain"))
            {
                Destroy(gameObject);
            }
        }
    }
}