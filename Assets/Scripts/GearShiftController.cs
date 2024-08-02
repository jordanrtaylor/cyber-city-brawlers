using UnityEngine;
using TMPro;

public class GearShiftController : MonoBehaviour
{
    public float baseMotorTorque = 25f; // Further reduced Torque
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gearText;
    public float speedCapMPH = 200f; // Speed cap in MPH
    public float accelerationFactor = 5f; // Further reduced acceleration factor
    public float reverseFactor = 4f; // Further reduced reverse factor
    public float dragFactor = 5f; // Drag factor for controlling speed

    private Rigidbody rb;
    private float currentSpeed;
    private int currentGear = 1;
    private float[] gearRatios = { 0.2f, 0.4f, 0.6f, 0.8f, 1f }; // More gradual gear ratios
    private float shiftDelay = 1f; // Increased delay for gear shifts
    private float nextShiftTime = 0f; // Time when the next gear shift can occur
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Debug logs to identify null references
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned.");
        }
        if (speedText == null)
        {
            Debug.LogError("SpeedText is not assigned.");
        }
        if (gearText == null)
        {
            Debug.LogError("GearText is not assigned.");
        }
    }

    void Update()
    {
        if (rb == null || speedText == null || gearText == null)
        {
            // Skip execution if any reference is missing
            return;
        }

        CheckIfGrounded();
        HandleMotor();
        HandleGearShift();
        UpdateUI();
    }

    private void CheckIfGrounded()
    {
        // Cast multiple rays to check if the car is grounded
        Vector3[] raycastPositions = new Vector3[]
        {
            transform.position + new Vector3(0, 0, 0), // Center
            transform.position + new Vector3(1, 0, 1), // Front-right
            transform.position + new Vector3(-1, 0, 1), // Front-left
            transform.position + new Vector3(1, 0, -1), // Rear-right
            transform.position + new Vector3(-1, 0, -1) // Rear-left
        };

        isGrounded = false;
        foreach (var pos in raycastPositions)
        {
            if (Physics.Raycast(pos, -Vector3.up, 1.1f))
            {
                isGrounded = true;
                break;
            }
        }
        Debug.Log("Is Grounded: " + isGrounded);
    }

    private void HandleMotor()
    {
        float gas = Input.GetAxis("Gas"); // Assuming "Gas" is set up in the Input Manager for the right trigger
        float brake = Input.GetAxis("Brake"); // Assuming "Brake" is set up in the Input Manager for the left trigger

        if (isGrounded)
        {
            rb.AddForce(transform.forward * accelerationFactor * gas);
            rb.AddForce(-transform.forward * reverseFactor * brake);
        }

        // Apply drag to slow down the car when no input is given
        if (gas == 0 && brake == 0)
        {
            rb.drag = dragFactor;
        }
        else
        {
            rb.drag = 0;
        }

        Debug.Log("Gas: " + gas + ", Brake: " + brake + ", Drag: " + rb.drag);
    }

    private void HandleGearShift()
    {
        currentSpeed = rb.velocity.magnitude * 2.23694f; // Convert m/s to MPH

        // Cap the speed
        if (currentSpeed > speedCapMPH)
        {
            rb.velocity = rb.velocity.normalized * (speedCapMPH / 2.23694f); // Convert MPH back to m/s
            currentSpeed = speedCapMPH;
        }

        // Implement a delay before the next gear shift
        if (Time.time >= nextShiftTime)
        {
            // Gear shifting based on the specified intervals
            if (currentSpeed >= 140 && currentGear < gearRatios.Length)
            {
                currentGear++;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 100 && currentSpeed < 140 && currentGear != 4)
            {
                currentGear = 4;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 70 && currentSpeed < 100 && currentGear != 3)
            {
                currentGear = 3;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 50 && currentSpeed < 70 && currentGear != 2)
            {
                currentGear = 2;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed < 50 && currentGear != 1)
            {
                currentGear = 1;
                nextShiftTime = Time.time + shiftDelay;
            }
        }

        // Set the motor torque based on the current gear
        float motorTorque = baseMotorTorque * gearRatios[currentGear - 1];
        if (isGrounded)
        {
            rb.AddForce(transform.forward * motorTorque);
        }

        Debug.Log("Current Gear: " + currentGear + ", Motor Torque: " + motorTorque + ", Current Speed: " + currentSpeed);
    }

    private void UpdateUI()
    {
        if (speedText != null && gearText != null)
        {
            speedText.text = $"Speed: {currentSpeed:F1} MPH";
            gearText.text = $"Gear: {currentGear}";
        }
    }
}
