using UnityEngine;

namespace ludum
{
    public class Main : MonoBehaviour
    {
        public float playerSpeed = 5f;

        public float fallSpeed = 0f;

        public float gravityAcceleration = 9.8f;

        public float maxFallSpeedNoInput = 3f;
        public float maxFallSpeedWithInput = 7f;

        public float hoverAcceleration = 20f;

        private bool isDead = false;

        public void SetDead()
        {
            isDead = true;
        }

        void Update()
        {
            if (isDead) return;

            float horizontal = Input.GetAxis("Horizontal");
            float horizontalMovement = horizontal * playerSpeed;

            if (Input.GetKey(KeyCode.W))
            {
                fallSpeed = Mathf.MoveTowards(fallSpeed, 0f, hoverAcceleration * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                fallSpeed += gravityAcceleration * 2f * Time.deltaTime;
                fallSpeed = Mathf.Min(fallSpeed, maxFallSpeedWithInput);
            }
            else
            {
                fallSpeed += gravityAcceleration * Time.deltaTime;
                fallSpeed = Mathf.Min(fallSpeed, maxFallSpeedNoInput);
            }

            Vector2 movement = new Vector2(horizontalMovement, -fallSpeed);
            transform.Translate(movement * Time.deltaTime, Space.World);

            Debug.Log(gameObject.name + $" Movement Speed：{movement}");
        }
    }
}