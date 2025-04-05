using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public bool isAlert;
    // Start is called before the first frame update
    void Start()
    {
        isAlert = false;
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
