using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed;
    public readonly float GRAVITY = -9.81f;
    public float fallMultiplier = 2.5f;
    public float jumpMultiplier = 2f;
    public float jumpForce;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    
    void Start()
    {
        
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * GRAVITY);
        }

        if(velocity.y < 0)
        {
            velocity += Vector3.up * GRAVITY * (fallMultiplier - 1) * Time.deltaTime;
        } else if (velocity.y > 0 && !Input.GetButtonDown("Jump"))
        {
            velocity += Vector3.up * GRAVITY * (jumpMultiplier - 1) * Time.deltaTime;
        }
    }
}