using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraWallFade : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float fadeAlpha = 0.3f;
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private LayerMask wallMask;

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
}
