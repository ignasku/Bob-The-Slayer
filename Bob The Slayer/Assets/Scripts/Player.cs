using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 100f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;

    public float health;

    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
        Debug.Log("Player has " + health + " hp");
    }

    private void Die()
    {
        Debug.Log("You have Died");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Game Over");
    }

    // Update is called once per frame
    void Update()
    {

       

        if (Input.GetKeyDown("left shift"))
        {
            speed *= 1.5f;
        }
        if (Input.GetKeyUp("left shift"))
        {
            speed = speed * 2 / 3f;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal") * 4;
        float z = Input.GetAxis("Vertical") * 4;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
