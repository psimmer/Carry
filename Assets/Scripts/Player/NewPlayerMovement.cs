using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [Header("PlayerSpeed")] [SerializeField] float playerSpeed;
    CharacterController characterController;
    Animator animator;

    float gravity;
    Vector3 velocity;
    bool isPlayerGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }


    void PlayerMovement()
    {
        isPlayerGrounded = characterController.isGrounded;
        if (isPlayerGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            characterController.Move((movement * playerSpeed) / 1.25f);
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            characterController.Move((movement * playerSpeed) / 1.25f);
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            characterController.Move((movement * playerSpeed) / 1.25f);
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            characterController.Move((movement * playerSpeed) / 1.25f);
        else
            characterController.Move(movement * playerSpeed);

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            animator.SetBool("isWalking", true);
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            animator.SetBool("isWalking", false);


        if (movement != Vector3.zero)
        {
            gameObject.transform.forward = movement;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }



}
