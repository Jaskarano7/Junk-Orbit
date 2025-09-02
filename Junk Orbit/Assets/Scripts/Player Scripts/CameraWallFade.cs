using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraWallFade : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float fadeAlpha = 0.3f;
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private LayerMask wallMask;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float smoothSpeed = 5f;

    private Renderer currentWall;
    private Material wallMaterial;
    private Color originalColor;

    void Update()
    {
        // Cast ray from camera to player
        Vector3 direction = player.position - transform.position;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude, wallMask))
        {
            if (hit.collider != null)
            {
                Renderer wall = hit.collider.GetComponent<Renderer>();

                if (wall != null)
                {
                    if (currentWall != wall)
                    {
                        RestoreWall(); // reset old wall
                        currentWall = wall;
                        wallMaterial = currentWall.material; // instance of material
                        originalColor = wallMaterial.color;
                    }

                    // Fade to transparent
                    Color c = wallMaterial.color;
                    c.a = Mathf.Lerp(c.a, fadeAlpha, Time.deltaTime * fadeSpeed);
                    wallMaterial.color = c;
                }
            }
        }
        else
        {
            RestoreWall();
        }
    }

    void RestoreWall()
    {
        if (currentWall != null && wallMaterial != null)
        {
            Color c = wallMaterial.color;
            c.a = Mathf.Lerp(c.a, originalColor.a, Time.deltaTime * fadeSpeed);
            wallMaterial.color = c;

            if (Mathf.Abs(c.a - originalColor.a) < 0.01f)
            {
                wallMaterial.color = originalColor;
                currentWall = null;
                wallMaterial = null;
            }
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Desired position
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // transform.LookAt(target);
    }
}
