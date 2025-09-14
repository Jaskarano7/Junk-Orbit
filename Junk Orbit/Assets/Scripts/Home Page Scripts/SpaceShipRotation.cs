using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceShipRotation : MonoBehaviour
{
    [Header("GameObjects To Rotate")]
    [SerializeField] private Transform dragRotateObject;  // Camera (or whatever rotates with drag)
    [SerializeField] private Transform buttonRotateObject; // Spaceship

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float inertia = 5f;

    [Header("Button Control")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private float[] targetRotations; // Local target rotations relative to camera
    [SerializeField] private Button LaunchButton;

    private bool isDragging;
    private Vector2 lastPointerPosition;
    private float rotationVelocity;
    private float currentTargetRotation;
    private bool useTargetRotation;

    private float initialCameraYRotation;

    void Start()
    {
        if (buttons.Length != targetRotations.Length)
        {
            Debug.LogError("Buttons array and targetRotations array must have the same length!");
            return;
        }

        // Store initial camera rotation
        if (dragRotateObject != null)
            initialCameraYRotation = dragRotateObject.eulerAngles.y;

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => RotateToTarget(index));
        }

        LaunchButton.onClick.AddListener(ToGame);
    }

    void Update()
    {
        HandleButtonRotation();
        HandleDragRotation();
    }

    private void HandleButtonRotation()
    {
        if (!useTargetRotation || buttonRotateObject == null) return;

        float newY = Mathf.SmoothDampAngle(buttonRotateObject.eulerAngles.y, currentTargetRotation, ref rotationVelocity, 0.3f);
        buttonRotateObject.rotation = Quaternion.Euler(buttonRotateObject.eulerAngles.x, newY, buttonRotateObject.eulerAngles.z);

        if (Mathf.Abs(Mathf.DeltaAngle(buttonRotateObject.eulerAngles.y, currentTargetRotation)) < 0.1f)
        {
            useTargetRotation = false;
            rotationVelocity = 0;
        }
    }

    private void HandleDragRotation()
    {
        if (useTargetRotation || dragRotateObject == null) return;

        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            isDragging = true;
            lastPointerPosition = Pointer.current.position.ReadValue();
            rotationVelocity = 0;
        }
        else if (Pointer.current.press.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 currentPosition = Pointer.current.position.ReadValue();
            float deltaX = currentPosition.x - lastPointerPosition.x;

            rotationVelocity = deltaX * rotationSpeed;
            dragRotateObject.Rotate(Vector3.up, rotationVelocity * Time.deltaTime, Space.World);

            lastPointerPosition = currentPosition;
        }
        else
        {
            if (Mathf.Abs(rotationVelocity) > 0.01f)
            {
                dragRotateObject.Rotate(Vector3.up, rotationVelocity * Time.deltaTime, Space.World);
                rotationVelocity = Mathf.Lerp(rotationVelocity, 0, Time.deltaTime * inertia);
            }
        }
    }

    private void RotateToTarget(int index)
    {
        if (index >= 0 && index < targetRotations.Length)
        {
            // Calculate target rotation relative to camera's current rotation
            float currentCameraY = dragRotateObject != null ? dragRotateObject.eulerAngles.y : 0f;
            float cameraDelta = currentCameraY - initialCameraYRotation;

            // Add camera delta so spaceship rotation is relative to camera’s new orientation
            currentTargetRotation = targetRotations[index] + cameraDelta;

            useTargetRotation = true;
            rotationVelocity = 0;
            isDragging = false;
        }
    }

    void ToGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

}
