using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [Header("PlayerSpeed")] [SerializeField] float playerSpeed;
    public float PlayerSpeed { get { return playerSpeed; } set { playerSpeed = value; } }

    Animator animator;
    public Animator PlayerAnimator { get { return animator; } set { animator = value; } }

    CharacterController characterController;
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

        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        movement = Vector3.ClampMagnitude(movement, 1) * playerSpeed * Time.deltaTime;

        characterController.Move(movement);

        if (movement.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
            SoundManager.instance.PlayAudioClip(ESoundeffects.FootSteps, GetComponent<AudioSource>());
        }

        if (movement.magnitude <= 0)
        {
            animator.SetBool("isWalking", false);
        }

        if (movement != Vector3.zero)
        {
            gameObject.transform.forward = movement;
        }

    }



}
