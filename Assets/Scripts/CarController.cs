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
    private float motorTorque;

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
        HandleMotor();
        HandleSteering();
        UpdateWheelPoses();
    }

    private void HandleMotor()
    {
        rb.AddForce(transform.forward * motorTorque * Input.GetAxis("Vertical"));
    }

    private void HandleSteering()
    {
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        Vector3 turn = new Vector3(0, steering, 0);
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

    public void SetMotorTorque(float torque)
    {
        motorTorque = torque;
    }
}
