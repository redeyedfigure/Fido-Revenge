using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Game Objects")]
    public CharacterController controller;
    public Shooting shootingScript;

    [Header("Player Stats")]
    public float regularSpeed = 10f;
    public float aimingSpeed = 6f;
    public readonly float GRAVITY = -9.81f;
    public float fallMultiplier = 2.5f;
    public float jumpMultiplier = 2f;
    public float jumpForce;

    private float currentSpeed;

    [Header ("Ground Checking")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    [Header("Pick Ups")]
    public LayerMask pickup;
    

    void Update()
    {
        if(shootingScript.isAiming)
        {
            currentSpeed = aimingSpeed;
        }
        else
        {
            currentSpeed = regularSpeed;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
        
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