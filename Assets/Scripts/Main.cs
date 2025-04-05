using UnityEngine;

namespace ludum
{
    public class Main : MonoBehaviour
    {
        public Animator anim;
        public float playerSpeed = 5f;

        void Start()
        {
            anim = GetComponent<Animator>();

            if (anim == null)
                Debug.LogError("未找到Animator组件，请检查Player上是否挂载了Animator");

            if (!gameObject.CompareTag("Player"))
            {
                gameObject.tag = "Player";
            }

            anim.speed = 0.5f;
        }

        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, vertical, 0);

            transform.Translate(movement * playerSpeed * Time.deltaTime, Space.World);

            if (movement.magnitude > 0)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    }
}