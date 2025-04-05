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
            alertDistance = 3f;
        }

        void Update()
        {
            if (Time.timeScale == 0f) return; // 增加判断确保游戏暂停时不再运行

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
    }
}