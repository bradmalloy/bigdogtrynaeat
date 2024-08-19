using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 0.5f;
    [SerializeField] private int score = 0;
    [SerializeField] private AudioSource goodFoodSound;
    [SerializeField] private AudioSource badFoodSound;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private const float scaleFactor = 1.25f; // 25% scale factor
    [SerializeField] private int[] scoreTargets;

    private Animator animObj;
    private Rigidbody rb;

    private bool isWalking;
    private bool isJumping;

    public void Move(Vector3 direction, bool isJumping, Vector2 mouseDelta)
    {

        if (direction != Vector3.zero)
        {
            animObj.SetBool("isWalking", true);
        }
        else {
            animObj.SetBool("isWalking", false);
        }

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
        
        // Rotate the player to face the direction of movement
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move the player
        Vector3 move = desiredMoveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
        
        // If we're inputting a jump, make the player jump
        // Handle jumping
        if (isJumping && isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    private void Update() {
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animObj = GetComponentInChildren<Animator>();
    }

    public void Init(Transform cameraRig)
    {
        cameraTransform = cameraRig;
    }

    private void OnCollisionEnter(Collision collision) {
        // Get the food's value and add it to our score -> placeholder is 1pt
        
        // if we reach a certain size category, update to bigger or smaller model
        if (collision.collider.gameObject.CompareTag("Good Food")) {
            goodFoodSound.Play();
            ChangeScore(collision.collider.gameObject.GetComponent<FoodScript>().score);
        } 
        else if (collision.collider.gameObject.CompareTag("Bad Food")) { 
            badFoodSound.Play();
            // Food scores are currently always positive, even for bad food
            ChangeScore(collision.collider.gameObject.GetComponent<FoodScript>().score * -1);
        }
    }

    // Change the stored score value and check if we need to change his size
    private void ChangeScore(int pointsToChange)
    {
        int previousScore = score;
        int newScore = score + pointsToChange;
        score = newScore;

        foreach (int target in scoreTargets)
        {
            if (previousScore <= target && newScore > target)
            {
                IncreaseSize();
            } else if (previousScore > target && newScore <= target)
            {
                DecreaseSize();
            }
        }
    }

    // Method to increase the size of the parent GameObject by 25%
    public void IncreaseSize()
    {
        transform.localScale *= scaleFactor;
    }

    // Method to decrease the size of the parent GameObject by 25%
    public void DecreaseSize()
    {
        transform.localScale /= scaleFactor;
    }

    public bool IsWalking() {
        return isWalking;
    }

    public bool IsJumping() { 
        return isJumping; 
    }

    private bool isGrounded()
    {
        Vector3 boxCenter = transform.position;
        
        Vector3 boxHalfExtents = transform.localScale / 2f;
        
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity);
        
        // Check if any of the colliders have the tag "IsJumpable"
        bool isTouchingJumpable = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("IsJumpable"))
            {
                isTouchingJumpable = true;
                break;
            }
        }
        
        if (isTouchingJumpable)
        {
            return true;
        }

        return false;
    }
    
}
