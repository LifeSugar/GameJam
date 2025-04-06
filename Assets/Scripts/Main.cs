using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace ludum
{
    public class Main : MonoBehaviour
    {
        [Header("Player Movement")]
        public float playerSpeed = 5f;
        public float fallSpeed = 0f;
        public float gravityAcceleration = 9.8f;
        public float maxFallSpeedNoInput = 3f;
        public float maxFallSpeedWithInput = 7f;
        public float hoverAcceleration = 20f;

        [Header("Energy System")]
        public float maxEnergy = 100f;
        public float currentEnergy = 100f;
        public float energyDecaySpeed = 20f;
        public float energyRecoverSpeed = 10f;
        public Slider energySlider;

        [Header("Special Ability")]
        public float cooldownTime = 5f;
        private bool isCooldown = false;
        private Coroutine cooldownCoroutine;

        [Header("Visual Effects")]
        public GameObject winText;
        public UniversalRendererData data;
        private FullScreenPassRendererFeature fullScreenRF;

        private bool isDead = false;
        private bool hasWon = false;

        public float lightTime = 3f;

        private void Start()
        {
            fullScreenRF = data.rendererFeatures.OfType<FullScreenPassRendererFeature>().FirstOrDefault();
            if (fullScreenRF != null)
            {
                fullScreenRF.passMaterial.SetFloat("_OutlineStrength", 0f);
            }
        }

        private void Update()
        {
            if (isDead || hasWon) return;

            HandleSpecialAbility();
            HandleMovement();
            CheckWinCondition();
        }

        private void HandleSpecialAbility()
        {
            // 按下空格键且不在冷却时触发技能
            if (Input.GetKeyDown(KeyCode.Space) && !isCooldown)
            {
                // 启动技能效果
                if (fullScreenRF != null)
                {
                    fullScreenRF.passMaterial.SetFloat("_OutlineStrength", 0.5f);
                }

                // 启动冷却协程
                cooldownCoroutine = StartCoroutine(CooldownRoutine());
            }
        }

        private IEnumerator CooldownRoutine()
        {
            isCooldown = true;
            float timer = 0f;

            while (timer < cooldownTime)
            {
                timer += Time.deltaTime;
                if (timer > lightTime)
                {
                    fullScreenRF.passMaterial.SetFloat("_OutlineStrength", 0f);
                }
                yield return null;
            }

            // 冷却结束后重置效果
            //if (fullScreenRF != null)
            //{
            //    fullScreenRF.passMaterial.SetFloat("_OutlineStrength", 0f);
            //}
            isCooldown = false;
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float horizontalMovement = horizontal * playerSpeed;

            bool isHoldingW = Input.GetKey(KeyCode.W);

            HandleVerticalMovement(isHoldingW);
            UpdateEnergy(isHoldingW);

            Vector2 movement = new Vector2(horizontalMovement, -fallSpeed);
            transform.Translate(movement * Time.deltaTime, Space.World);

            UpdateEnergyUI();
        }

        private void HandleVerticalMovement(bool isHoldingW)
        {
            if (isHoldingW && currentEnergy > 0f)
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
        }

        private void UpdateEnergy(bool isHoldingW)
        {
            if (isHoldingW)
            {
                currentEnergy -= energyDecaySpeed * Time.deltaTime;
                currentEnergy = Mathf.Max(currentEnergy, 0f);
            }
            else
            {
                currentEnergy += energyRecoverSpeed * Time.deltaTime;
                currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            }
        }

        private void UpdateEnergyUI()
        {
            if (energySlider != null)
            {
                energySlider.value = currentEnergy;
            }
        }

        private void CheckWinCondition()
        {
            if (transform.position.y < -180f)
            {
                Win();
            }
        }

        public void SetDead()
        {
            isDead = true;
        }

        private void Win()
        {
            hasWon = true;
            if (winText != null)
            {
                winText.SetActive(true);
            }
        }
    }
}
