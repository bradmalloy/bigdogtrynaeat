using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private int score = 0;
    [SerializeField] private AudioSource goodFoodSound;
    [SerializeField] private AudioSource badFoodSound;
    [SerializeField] private Transform cameraTransform;

    private bool isWalking;
    private bool isJumping;

    public void Move(Vector3 direction, bool isJumping, Vector2 mouseDelta)
    {
        // Vector3 inputVector = new Vector3(0, 0, 0);
        // inputVector = direction.normalized;
        // Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, inputVector.z);
        // transform.position += moveDir * moveSpeed * Time.deltaTime;
        //
        // isWalking = moveDir != Vector3.zero;
        // if (!isJumping) {
        //     float rotateSpeed = 10f;
        //     transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        // }
        //
        // isJumping = false;
        
        // Handle the camera rotation based on mouse input
        float mouseX = mouseDelta.x;
        transform.Rotate(Vector3.up * mouseX);

        // Adjust the direction relative to the camera's forward direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;  // Keep the forward vector horizontal
        right.y = 0f;    // Keep the right vector horizontal
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * direction.z + right * direction.x;
        desiredMoveDirection.Normalize();

        // Move the player
        controller.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);

        // Check if the player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        // Handle jumping
        if (isGrounded && isJumping)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply the vertical velocity
        controller.Move(velocity * Time.deltaTime);
    }
    
    private void Update() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        // Get the food's value and add it to our score -> placeholder is 1pt
        
        // if we reach a certain size category, update to bigger or smaller model
        if (collision.collider.gameObject.CompareTag("Good Food")) {
            goodFoodSound.Play();
            score += collision.collider.gameObject.GetComponent<FoodScript>().score;
        } 
        else if (collision.collider.gameObject.CompareTag("Bad Food")) { 
            badFoodSound.Play();
            score -= collision.collider.gameObject.GetComponent<FoodScript>().score;
        }
        print(score);
    }


    public bool IsWalking() {
        return isWalking;
    }

    public bool IsJumping() { 
        return isJumping; 
    }
    
}
