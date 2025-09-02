using UnityEngine;

public class MeteoridMotion : MonoBehaviour
{
    [SerializeField] private float floatAmplitude = 0.5f; // how high/low it floats
    [SerializeField] private float floatSpeed = 1f;       // speed of floating
    [SerializeField] private float rotationSpeed = 30f;   // degrees per second

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Give each meteorid a random rotation speed and direction
        rotationSpeed = Random.Range(-rotationSpeed, rotationSpeed);
        floatSpeed = Random.Range(0.5f * floatSpeed, 1.5f * floatSpeed);
        floatAmplitude = Random.Range(0.5f * floatAmplitude, 1.5f * floatAmplitude);
    }

    void Update()
    {
        // Floating (up & down using sine wave)
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Rotation
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
