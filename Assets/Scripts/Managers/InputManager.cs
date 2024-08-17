using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager
{
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    // private Player thePlayer = null;
    
    private static InputManager instance = null;

    // public void SetPlayer(Player referenceToPlayer)
    // {
    //     // Change the type of thePlayer to Player once it exists
    //     thePlayer = referenceToPlayer;
    // }

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

        bool jump = Input.GetKey(jumpKey);

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), 0); // Only horizontal rotation

        // thePlayer.Move(direction, jump, mouseDelta);
    }

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }
    
    // Intentionally blank
    private InputManager() {}
}