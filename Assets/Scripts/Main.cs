using UnityEngine;
using UnityEngine.UI; //注意添加这个命名空间！

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

        public float maxEnergy = 100f;
        public float currentEnergy = 100f;
        public float energyDecaySpeed = 20f;
        public float energyRecoverSpeed = 10f;

        public Slider energySlider; // 引用UI Slider组件

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

            if (!isHoldingW)
            {
                currentEnergy += energyRecoverSpeed * Time.deltaTime;
                currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            }

            Vector2 movement = new Vector2(horizontalMovement, -fallSpeed);
            transform.Translate(movement * Time.deltaTime, Space.World);

            // 更新Slider显示
            if (energySlider != null)
                energySlider.value = currentEnergy;
        }
    }
}