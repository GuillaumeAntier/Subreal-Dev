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
        rb.linearVelocity = new Vector2(0,0);
        if (Input.GetKey("w")){
            rb.linearVelocity = new Vector2(0,1*movementSpeed);
        }
        if (Input.GetKey("a")){
            rb.linearVelocity = new Vector2(-1*movementSpeed,0);
        }
        if (Input.GetKey("s")){
            rb.linearVelocity = new Vector2(0,-1*movementSpeed);
        }
        if (Input.GetKey("d")){
            rb.linearVelocity = new Vector2(1*movementSpeed,0);
        }
        if (Input.GetKey("w") && Input.GetKey("d")){
            rb.linearVelocity = new Vector2(0.707f*movementSpeed,0.707f*movementSpeed);
        }
        if (Input.GetKey("w") && Input.GetKey("a")){
            rb.linearVelocity = new Vector2(-0.707f*movementSpeed,0.707f*movementSpeed);
        }
        if (Input.GetKey("s") && Input.GetKey("d")){
            rb.linearVelocity = new Vector2(0.707f*movementSpeed,-0.707f*movementSpeed);
        }
        if (Input.GetKey("s") && Input.GetKey("a")){
            rb.linearVelocity = new Vector2(-0.707f*movementSpeed,-0.707f*movementSpeed);
        }
        
    }
}
