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

        // 新增能量相关变量
        public float maxEnergy = 100f;
        public float currentEnergy = 100f;
        public float energyDecaySpeed = 20f;     // 每秒消耗速度
        public float energyRecoverSpeed = 10f;   // 每秒恢复速度

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

            bool isHoldingW = Input.GetKey(KeyCode.W);

            if (isHoldingW && currentEnergy > 0f)
            {
                fallSpeed = Mathf.MoveTowards(fallSpeed, 0f, hoverAcceleration * Time.deltaTime);
                currentEnergy -= energyDecaySpeed * Time.deltaTime;

                // 防止能量值降至0以下
                currentEnergy = Mathf.Max(currentEnergy, 0f);
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

            // 如果没有按住W，能量逐渐恢复
            if (!isHoldingW)
            {
                currentEnergy += energyRecoverSpeed * Time.deltaTime;

                // 能量不会超过最大值
                currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            }

            Vector2 movement = new Vector2(horizontalMovement, -fallSpeed);
            transform.Translate(movement * Time.deltaTime, Space.World);
        }
    }
}