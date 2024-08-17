using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private int score = 0;

    private bool isWalking;
    private bool isJumping;
    private void Update() {
        Vector3 inputVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) {
            inputVector.z = +1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.z = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = +1;
        }
        if (Input.GetKey(KeyCode.Space)) {
            inputVector.y = +2;
            isJumping = true;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, inputVector.z);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;
        if (!isJumping) {
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }

        isJumping = false;

    }

    private void OnCollisionEnter(Collision collision) {
        // Get the food's value and add it to our score -> placeholder is 1pt
        score += 1;
        // if we reach a certain size category, update to bigger or smaller model
    }

    public bool IsWalking() {
        return isWalking;
    }

    public bool IsJumping() { 
        return isJumping; 
    }




}
