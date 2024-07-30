using UnityEngine;
using TMPro; // Ensure you have TextMeshPro imported

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
    private float[] gearRatios = { 0.5f, 1f, 1.5f }; // Adjusted gear ratios for 3 gears

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

        // Gear shifting based on the specified intervals
        if (currentSpeed > 140 && currentGear < 3)
        {
            currentGear = 3;
        }
        else if (currentSpeed > 80 && currentGear < 2)
        {
            currentGear = 2;
        }
        else if (currentSpeed > 40 && currentGear < 1)
        {
            currentGear = 1;
        }
        else if (currentSpeed < 40 && currentGear > 1)
        {
            currentGear = 1;
        }
        else if (currentSpeed < 80 && currentGear > 2)
        {
            currentGear = 2;
        }
        else if (currentSpeed < 140 && currentGear > 3)
        {
            currentGear = 3;
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
