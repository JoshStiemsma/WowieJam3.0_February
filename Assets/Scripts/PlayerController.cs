using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public float thrust = 5;
    public float maxSpeed = 5f;
    bool grounded;

    public enum Player
    {
        Left,
        Right
    }

    public Player playerType;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(Input.GetKey("d"))
        {
           rb.AddForce(transform.right * thrust);
        }
         if(Input.GetKey("a"))
        {
           rb.AddForce(-transform.right * thrust);
        }
         if(Input.GetKeyDown("w") && grounded == true)
        {
           rb.AddForce(transform.up * (thrust * 100));
           grounded = false;
        }

         if(rb.velocity.magnitude > maxSpeed)
         {
                rb.velocity = rb.velocity.normalized * maxSpeed;
         }

        


    }
         void OnCollisionEnter2D(Collision2D col)
    {
       
       if (col.gameObject.tag == "Ground")
       {
           grounded = true;
        }
    }


}
