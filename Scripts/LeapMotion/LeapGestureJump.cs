using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapGestureJump : MonoBehaviour
{
    public GameObject player; // Drag your character object here
    public float jumpForce = 7f;

    private Controller leapController;
    private Rigidbody playerRb;

    void Start()
    {
        leapController = new Controller();
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Frame frame = leapController.Frame();

        if (!frame.Hands.IsEmpty)
        {
            Hand hand = frame.Hands[0]; // Use the first detected hand

            // Detect a strong grab (fist)
            if (hand.GrabStrength > 0.9f && IsGrounded())
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
        Debug.Log("Leap: Jump triggered!");
    }

    bool IsGrounded()
    {
        // Simple grounded check (customize based on your setup)
        return Physics.Raycast(player.transform.position, Vector3.down, 1.1f);
    }
}
