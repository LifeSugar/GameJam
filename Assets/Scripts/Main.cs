using UnityEngine;

namespace ludum
{
    public class Main : MonoBehaviour
    {
        public float playerSpeed = 5f;

        private bool isDead = false;

        public void SetDead()
        {
            isDead = true;
        }

        void Update()
        {
            Debug.Log(gameObject.name + " 正在Update移动！");

            if (isDead) return;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movement = new(horizontal, vertical);

            transform.Translate(playerSpeed * Time.deltaTime * movement, Space.World);
        }
    }
}