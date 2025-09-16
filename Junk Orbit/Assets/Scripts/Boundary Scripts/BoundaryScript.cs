using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    [Header("Boundary Settings")]
    [SerializeField] private GameObject[] meteoridPrefabs;
    [SerializeField] private float radius;
    [SerializeField] private int segments = 36; // number of points around the circle
    [SerializeField] private float wallThickness = 1f;
    [SerializeField] private float wallHeight = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    void Start()
    {
        CreateCircularBoundary();
        CreateCircularColliders();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void CreateCircularBoundary()
    {
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            SpawnMeteorid(pos, angle);
        }
    }

    void SpawnMeteorid(Vector3 position, float angle)
    {
        int index = Random.Range(0, meteoridPrefabs.Length);
        GameObject obj = Instantiate(meteoridPrefabs[index], position, Quaternion.identity, transform);

        // Optional: rotate meteor to face center
        obj.transform.LookAt(transform.position);
    }

    void CreateCircularColliders()
    {
        GameObject walls = new GameObject("CircularBoundaryColliders");
        walls.transform.parent = transform;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Quaternion rot = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);

            GameObject wall = new GameObject("WallSegment");
            wall.transform.parent = walls.transform;
            wall.transform.localPosition = pos;
            wall.transform.localRotation = rot;

            wall.tag = "Obstacle";

            // BoxCollider approximating a small arc segment
            BoxCollider col = wall.AddComponent<BoxCollider>();
            col.size = new Vector3(wallThickness, wallHeight, (2 * Mathf.PI * radius) / segments);
            col.isTrigger = false;
        }
    }

    public float BoundaryDimension
    {
        get { return radius; }
    }
}
