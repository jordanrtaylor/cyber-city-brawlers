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
    private float[] gearRatios = { 0.5f, 0.75f, 1f, 1.25f, 1.5f }; // Adjusted gear ratios

    void Start()
    {
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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

        if (currentSpeed > 20 * currentGear && currentGear < gearRatios.Length)
        {
            currentGear++;
        }
        else if (currentSpeed < 15 * (currentGear - 1) && currentGear > 1)
        {
            currentGear--;
        }

        carController.SetMotorTorque(baseMotorTorque * gearRatios[currentGear - 1]);
    }

    private void UpdateUI()
    {
        speedText.text = $"Speed: {currentSpeed:F1} MPH";
        gearText.text = $"Gear: {currentGear}";
    }
}
