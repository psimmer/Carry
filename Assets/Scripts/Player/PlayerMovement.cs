using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Animator playerAnimator;

    private float horizontalMovement;
    private float verticalMovement;
    private float verticalRotation;
    private float horizontalRotation;
    Rigidbody playerRigidbody;
    Transform playerTransform;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerTransform = transform;
    }
    void Update()
    {
        playerMovement();
    }


    void playerMovement()
    {
        horizontalRotation = Input.GetAxis("Horizontal");
        verticalRotation = Input.GetAxis("Vertical");
        horizontalMovement = 0;
        verticalMovement = 0;

        if (Input.GetKey(KeyCode.W))
        {
            verticalMovement = 1f;
            playerAnimator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalMovement = -1f;
            playerAnimator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontalMovement = -1f;
            playerAnimator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalMovement = 1f;
            playerAnimator.SetBool("isWalking", true);
        }
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            playerAnimator.SetBool("isWalking", false);
        }

        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement);
        Vector3 rotation = new Vector3(horizontalRotation, 0f, verticalRotation);
        movement = movement.normalized * Time.deltaTime * playerMovementSpeed;

        if (rotation != Vector3.zero)
        {
            if (Mathf.Abs(horizontalMovement) > 0.17 || Mathf.Abs(verticalMovement) > 0.17)
            {
                playerTransform.rotation = Quaternion.LookRotation(rotation);
            }
        }

        if (playerRigidbody.velocity.magnitude < maxSpeed)
        {
            playerRigidbody.AddForce(movement);
        }

        //animator.setfloat("movement", playerRigidbody.velocity.magnitude)
    }
}
