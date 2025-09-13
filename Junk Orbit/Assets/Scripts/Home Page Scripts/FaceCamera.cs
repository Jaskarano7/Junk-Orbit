using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Make the object face the camera correctly
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
