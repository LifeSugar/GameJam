using UnityEngine;

namespace ludum
{
    public class Main : MonoBehaviour
    {
        public float playerSpeed = 5f;

        private bool isDead = false;       // 新增死亡标识符

        public void SetDead()              // 外部脚本调用设置死亡
        {
            isDead = true;
        }

        void Update()
        {
            Debug.Log(gameObject.name + " 正在Update移动！");  // 加这一行，打印每帧在动的是哪个游戏物体

            if (isDead) return;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movement = new(horizontal, vertical);

            transform.Translate(playerSpeed * Time.deltaTime * movement, Space.World);
        }
    }
}