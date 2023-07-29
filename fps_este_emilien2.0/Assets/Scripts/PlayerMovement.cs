using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    private float speed = 12f;
    public float gravity = -9.810f;
    public float jumpHeight = 3f;
    public float currentSpeed;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float sprintDuration = 5f;
    bool sprint;


    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        if (Input.GetButtonDown("Shift") && isGrounded)
        {
            currentSpeed = 2 * speed;
            print("shift");
            sprint = true;

        }


        else if (Input.GetButtonUp("Shift") && isGrounded)
        {
            currentSpeed = speed;
            sprint = false;

        }

        if (Input.GetButtonDown("tab") && !sprint)
        {
            currentSpeed = 100 * speed;
            print("aaaaa");
        }

        else if (!Input.GetButtonDown("tab") && !sprint)
        {
            currentSpeed = speed;
        }






        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}