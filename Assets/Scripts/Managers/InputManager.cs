using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : MonoBehaviour
{
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode barkKey = KeyCode.E;
    [SerializeField] private AudioSource barkSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioClip barkClip;

    // The inverse of this is when we're in a menu
    private bool gameIsActive = false;

    private PlayerScript thePlayer;

    public void Init(PlayerScript player)
    {
        Debug.Log("InputManager starting up, assigning player reference.");
        thePlayer = player;
        this.gameObject.SetActive(true);
        if (thePlayer != null)
        {
            Debug.Log("Looks good.");
            gameIsActive = true;
        }
        else
        {
            Debug.LogError("Uh oh, no PlayerScript was provided?");
        }
        
    }

    private void Start()
    {
        if (thePlayer == null)
        {
            Debug.LogError("Player component not found, InputManager can't start!");
        }
    }
    
    private void Update()
    {
        if (gameIsActive)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(forwardKey))
            {
                direction += Vector3.forward;
            }
            if (Input.GetKey(backwardKey))
            {
                direction += Vector3.back;
            }
            if (Input.GetKey(leftKey))
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(rightKey))
            {
                direction += Vector3.right;
            }

            if (Input.GetKey(jumpKey))
            {
                direction += Vector3.up;
            }

            if (Input.GetKey(barkKey))
            {
                barkSound.Play();
                Debug.Log("He bark");
            }

            bool jump = Input.GetKey(jumpKey);
            if (jump)
            {
                jumpSound.Play();
            }
 
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), 0); // Only horizontal rotation

            thePlayer.Move(direction, jump, mouseDelta);
        }
    }

    public void ToggleEnabled()
    {
        gameIsActive = !gameIsActive;
    }
}