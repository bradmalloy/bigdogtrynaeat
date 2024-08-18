using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int score = 0;
    [SerializeField] private AudioSource goodFoodSound;
    [SerializeField] private AudioSource badFoodSound;
    [SerializeField] private Transform cameraTransform;
    private Rigidbody rb;

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
        
        // Rotate the player to face the direction of movement
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move the player
        Vector3 move = desiredMoveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }
    
    private void Update() {
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
