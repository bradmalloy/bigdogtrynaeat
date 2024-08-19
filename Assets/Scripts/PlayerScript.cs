using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 16f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private int score = 0;
    [SerializeField] private AudioSource goodFoodSound;
    [SerializeField] private AudioSource badFoodSound;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private const float scaleFactor = 1.25f; // 25% scale factor
    [SerializeField] private int[] scoreTargets;

    private Vector3 directionMovingIn;
    public float acceleration = 5f; // Adjust the acceleration rate
    private Vector3 currentVelocity = Vector3.zero;
    public float fallMultiplier = 2.5f; // Multiplier to increase gravity during fall
    public float lowJumpMultiplier = 2f; // Multiplier for shorter, quicker jumps
    public float jumpCooldownTime = 0.1f; // Grace period in seconds
    private float lastLandTime = 0f; // Tracks the last time the player jumped
    private bool canJump = true; // Indicates if the player can jump
    
    private Animator animObj;
    private GameTimer gameTimer;
    private int lastIdleTimeMark = 180;
    private Rigidbody rb;

    private bool isWalking;
    private bool isJumping;

    public void Move(Vector3 direction, bool isJumpInput, Vector2 mouseDelta)
    {

        if (direction != Vector3.zero)
        {
            animObj.SetBool("isWalking", true);
            lastIdleTimeMark = gameTimer.GetRemainingSeconds();
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
        directionMovingIn = move; // This will set the direction we WANT to move in,
        // and Update() will actually move the player (with acceleration and decel)

        // If we're inputting a jump, make the player jump
        // Handle jumping
        isJumping = isJumpInput;
    }
    
    private void FixedUpdate()
    {
        //Handle IdleTime animation activation
        Debug.Log("This is the current Idle Timer " + (lastIdleTimeMark - gameTimer.GetRemainingSeconds()));
        animObj.SetInteger("SitTimer", lastIdleTimeMark - gameTimer.GetRemainingSeconds());

        // Calculate the target horizontal velocity (ignoring vertical movement)
        Vector3 targetHorizontalVelocity = directionMovingIn * moveSpeed;

        // Smoothly interpolate the current horizontal velocity towards the target velocity
        Vector3 horizontalVelocity = Vector3.Lerp(new Vector3(currentVelocity.x, 0, currentVelocity.z), 
            targetHorizontalVelocity, 
            acceleration * Time.fixedDeltaTime);

        // Preserve the current vertical velocity (y component)
        currentVelocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

        // Apply the movement to the player (horizontal only)
        Vector3 move = currentVelocity * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Handle jumping and cooldown
        if (isGrounded())
        {
            if (Time.time - lastLandTime > jumpCooldownTime)
            {
                canJump = true;
            }
        }
        else
        {
            lastLandTime = Time.time; // Update the last land time when in the air
        }

        // Handle jumping
        if (isJumping && isGrounded() && canJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset any vertical velocity before jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Record the time of the last jump
        }

        // Apply extra gravity when falling for a more satisfying jump
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Apply extra gravity if the player releases the jump button early
        else if (rb.velocity.y > 0 && !isJumping) 
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animObj = GetComponentInChildren<Animator>();
        // Freeze rotation on the X and Z axes to keep the player upright
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Init(Transform cameraRig, GameTimer timer)
    {
        cameraTransform = cameraRig;
        gameTimer = timer;
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
        Debug.Log("Changing score by: " + pointsToChange);
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

    public string GetScore()
    {
        return "" + score;
    }

    public string GetNextScoreTarget()
    {
        int nextTarget = FindNextTarget(scoreTargets, score);
        if (nextTarget == -1)
        {
            return "Win!";
        }
        return "" + nextTarget;
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
    
    int FindNextTarget(int[] scoreTargets, int score)
    {
        int nextTarget = int.MaxValue;

        foreach (int target in scoreTargets)
        {
            if (target > score && target < nextTarget)
            {
                nextTarget = target;
            }
        }

        return nextTarget == int.MaxValue ? -1 : nextTarget; // Return -1 if no higher target is found
    }

}
