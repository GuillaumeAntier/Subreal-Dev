using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHero : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movementSpeed = 1f;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = 0;
        float moveY = 0;
        
        if (Input.GetKey("z")) moveY = 1;
        if (Input.GetKey("s")) moveY = -1;
        if (Input.GetKey("a")) moveX = -1;
        if (Input.GetKey("d")) moveX = 1;
        
        Vector2 direction = new Vector2(moveX, moveY);
        if (direction.magnitude > 0)
            direction = direction.normalized;
        
        rb.linearVelocity = direction * movementSpeed;
    }
}
