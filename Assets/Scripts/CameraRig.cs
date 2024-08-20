using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public Transform player;  // The player transform
    public float distance = 2.0f;  // Distance from the player
    public float height = 1.0f;  // Height above the player
    public float sensitivity = 2.0f;  // Mouse sensitivity for orbiting

    private float currentX = 0f;
    private float currentY = 0f;

    public void SetPlayer(GameObject toSet)
    {
        player = toSet.transform;
    }
    
    void Update()
    {
        if (player == null)
        {
            return;
        }
        
        // Get mouse input
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Clamp the Y rotation to prevent the camera from flipping over
        currentY = Mathf.Clamp(currentY, -35, 60);
    }

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }
        
        // Calculate the new camera position
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.position + rotation * direction;

        // Adjust the height
        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * height);
    }
}
