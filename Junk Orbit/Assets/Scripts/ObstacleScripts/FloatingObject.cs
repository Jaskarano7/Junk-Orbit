using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Floating Settings")]
    [SerializeField] private float floatAmplitude = 0.5f;   // how far up/down it moves
    [SerializeField] private float floatFrequency = 1f;     // how fast it moves

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f;     // degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // default rotation axis

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
        // Remember the initial position so it oscillates around it
        startPos = transform.position;

        // Add some randomness so not all floaters move the same
        randomOffset = Random.Range(0f, 100f);
        floatAmplitude *= Random.Range(0.3f, 0.7f);
        floatFrequency *= Random.Range(0.8f, 1.2f);

        // Randomize rotation axis slightly
        if (rotationAxis == Vector3.up) // only if you didn’t set a custom one
        {
            rotationAxis = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            ).normalized;
        }
    }

    void Update()
    {
        // Floating motion using sine wave
        float newY = startPos.y + Mathf.Sin((Time.time + randomOffset) * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Continuous slow rotation
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
