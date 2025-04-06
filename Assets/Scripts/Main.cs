using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace ludum
{
    public class Main : MonoBehaviour
    {
        public float playerSpeed = 5f;

        private bool isDead = false;
        public float fallSpeed = -1f;
        public float accelation = 1f;

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
            Vector2 movement = new(horizontal, fallSpeed - vertical * accelation);

            transform.Translate(playerSpeed * Time.deltaTime * movement, Space.World);


        }

    }
}