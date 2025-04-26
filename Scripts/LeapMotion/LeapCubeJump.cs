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

            if ((firstHand.PinchStrength > 0.8f || firstHand.GrabStrength > 0.8f) && isGrounded)
            {
                Jump();
            }
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
