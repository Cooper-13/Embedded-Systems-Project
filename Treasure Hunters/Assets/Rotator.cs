using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float turnSpeed = 360f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
           transform.Rotate(Vector3.left * turnSpeed);
        if (Input.GetKey(KeyCode.D))
           transform.Rotate(Vector3.right * turnSpeed);
    }

}