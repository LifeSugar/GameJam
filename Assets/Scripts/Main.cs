using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ludum {
    public class Main : MonoBehaviour
    {
        public float playerSpeed = 5f;
        // Start is called before the first frame update
        void Start()
        {
            if (!gameObject.CompareTag("Player"))
            {
                gameObject.tag = "Player";
            }
        }

        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, 0, vertical);
            transform.Translate(movement * playerSpeed * Time.deltaTime, Space.World);
        }
    }
}
