using UnityEngine;
using TMPro;

public class GearShiftController : MonoBehaviour
{
    public float baseMotorTorque = 1500f; // Base Torque the Motor can apply to Wheel
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gearText;
    public float speedCapMPH = 400f; // Speed cap in MPH

    private CarController carController;
    private Rigidbody rb;
    private float currentSpeed;
    private int currentGear = 1;
    private float[] gearRatios = { 0.5f, 0.75f, 1f, 1.25f, 1.5f, 1.75f }; // More realistic gear ratios
    private float shiftDelay = 0.5f; // Delay in seconds for gear shifts
    private float nextShiftTime = 0f; // Time when the next gear shift can occur

    void Start()
    {
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();

        // Debug logs to identify null references
        if (carController == null)
        {
            Debug.LogError("CarController is not assigned.");
        }
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
        if (carController == null || rb == null || speedText == null || gearText == null)
        {
            // Skip execution if any reference is missing
            return;
        }

        HandleGearShift();
        UpdateUI();
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
            if (currentSpeed >= 140 && currentGear < 6)
            {
                currentGear++;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 100 && currentSpeed < 140 && currentGear != 5)
            {
                currentGear = 5;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 70 && currentSpeed < 100 && currentGear != 4)
            {
                currentGear = 4;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 50 && currentSpeed < 70 && currentGear != 3)
            {
                currentGear = 3;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed >= 30 && currentSpeed < 50 && currentGear != 2)
            {
                currentGear = 2;
                nextShiftTime = Time.time + shiftDelay;
            }
            else if (currentSpeed < 30 && currentGear != 1)
            {
                currentGear = 1;
                nextShiftTime = Time.time + shiftDelay;
            }
        }

        carController.SetMotorTorque(baseMotorTorque * gearRatios[currentGear - 1]);
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
