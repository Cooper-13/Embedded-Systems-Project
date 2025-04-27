using UnityEngine;
using Leap;

public class LeapCubeJump : MonoBehaviour
{
    public LeapServiceProvider leapProvider; // Drag your LeapServiceProvider here
    private Rigidbody rb;
    public float jumpForce = 5f;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Frame frame = leapProvider.CurrentFrame;

        if (frame.Hands.Count > 0)
        {
            Hand firstHand = frame.Hands[0];

            // Option 1: Detect a Grab Gesture
            if ((firstHand.PinchStrength > 0.8f || firstHand.GrabStrength > 0.8f) && isGrounded)
            {
                Jump();
            }
            // Option 2: Detect a Palm Upward Gesture
            // if (firstHand.PalmPosition.y > 200 && isGrounded) // Adjust thresholds as needed
            // {
            //     Jump();
            // }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        Debug.Log("Leap: Jump triggered!");
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
