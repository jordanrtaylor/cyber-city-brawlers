using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSteeringAngle = 30f; // Maximum Steering Angle the Wheel can have
    public Transform centerOfMass; // Adjust the car's center of mass for stability
    public Transform wheelFrontLeft; // Front Left Wheel Transform
    public Transform wheelFrontRight; // Front Right Wheel Transform
    public Transform wheelBackLeft; // Back Left Wheel Transform
    public Transform wheelBackRight; // Back Right Wheel Transform

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }
    }

    void Update()
    {
        HandleSteering();
        UpdateWheelPoses();
    }

    private void HandleSteering()
    {
        float steer = maxSteeringAngle * Input.GetAxis("Horizontal");
        Vector3 turn = new Vector3(0, steer, 0);
        rb.AddTorque(turn);
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(wheelFrontLeft);
        UpdateWheelPose(wheelFrontRight);
        UpdateWheelPose(wheelBackLeft);
        UpdateWheelPose(wheelBackRight);
    }

    private void UpdateWheelPose(Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion quat = wheelTransform.rotation;

        wheelTransform.position = pos;
        wheelTransform.rotation = quat;
    }
}
