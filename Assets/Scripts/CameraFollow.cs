using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The car's transform
    public Vector3 offset = new Vector3(0, 5, -10);    // The offset distance between the camera and the target

    void LateUpdate()
    {
        // Simple follow without smoothing
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
