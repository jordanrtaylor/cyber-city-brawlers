using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The car's transform
    public Vector3 offset = new Vector3(0, 5, 10); // The default offset distance in front of the car
    public Vector3 rearViewOffset = new Vector3(0, 5, -10); // The offset for the rear view

    private bool isRearView = false;

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRearView = !isRearView;
        }

        if (isRearView)
        {
            // Default view (facing direction of the car)
            transform.position = target.position + target.TransformDirection(offset);
            transform.LookAt(target);
        }
        else
        {
            // Rear view
            transform.position = target.position + target.TransformDirection(rearViewOffset);
            transform.LookAt(target);
        }
    }
}
