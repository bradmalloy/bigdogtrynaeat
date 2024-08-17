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

    private PlayerScript thePlayer;

    public void Init(PlayerScript player)
    {
        Debug.Log("InputManager starting up, assigning player reference.");
        thePlayer = player;
        this.gameObject.SetActive(true);
        if (thePlayer != null)
        {
            Debug.Log("Looks good.");    
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
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(forwardKey))
        {
            direction += Vector3.forward;
            Debug.Log(forwardKey);
        }
        if (Input.GetKey(backwardKey))
        {
            direction += Vector3.back;
            Debug.Log(backwardKey);
        }
        if (Input.GetKey(leftKey))
        {
            direction += Vector3.left;
            Debug.Log(leftKey);
        }
        if (Input.GetKey(rightKey))
        {
            direction += Vector3.right;
            Debug.Log(rightKey);
        }

        if (Input.GetKey(jumpKey))
        {
            direction += Vector3.up;
            Debug.Log("He jump");
        }

        bool jump = Input.GetKey(jumpKey);

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), 0); // Only horizontal rotation

        thePlayer.Move(direction, jump, mouseDelta);
    }
}