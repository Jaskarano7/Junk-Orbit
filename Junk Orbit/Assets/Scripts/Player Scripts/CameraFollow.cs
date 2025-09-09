using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);

    [Header("Lag Settings")]
    [SerializeField] private bool useLag = true;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float rotationLag = 5f;

    [Header("Tilt Settings")]
    [SerializeField] private bool turningTilt = true;
    [SerializeField] private float tiltAmount = 5f;

    [Header("Rotation Options")]
    [SerializeField] private bool lookAtPlayer = true;

    private Vector3 velocity = Vector3.zero;
    private Rigidbody playerRb;

    private bool isShaking = false;
    void Start()
    {
        if (player != null)
            playerRb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // --- POSITION FOLLOW ---
        Vector3 targetPosition = player.position + offset;

        if (useLag)
        {
            // Smooth damp adds inertia feel
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                smoothTime
            );
        }
        else
        {
            // Instant follow
            transform.position = targetPosition;
        }

        // --- ROTATION ---
        if (lookAtPlayer)
        {
            Quaternion baseRotation = Quaternion.LookRotation(player.position - transform.position);

            if (turningTilt && playerRb != null)
            {
                Vector3 localVelocity = player.InverseTransformDirection(playerRb.linearVelocity);

                float tiltX = -localVelocity.z * 0.02f; // forward/back tilt
                float tiltZ = -localVelocity.x * 0.02f; // side tilt

                Quaternion tiltRot = Quaternion.Euler(tiltX * tiltAmount, 0f, tiltZ * tiltAmount);

                // Combine rotation + tilt
                baseRotation *= tiltRot;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, baseRotation, rotationLag * Time.deltaTime);
        }
    }

    public void ShakeCamera(float duration = 0.3f, float magnitude = 0.2f)
    {
        if (!isShaking)
            StartCoroutine(CameraShake(duration, magnitude));
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        isShaking = true;

        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isShaking = false;
    }
}
