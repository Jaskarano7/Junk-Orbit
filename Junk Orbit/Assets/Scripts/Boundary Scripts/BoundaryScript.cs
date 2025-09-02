using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    [Header("Boundary Settings")]
    [SerializeField] private GameObject[] meteoridPrefabs;
    [SerializeField] private Vector2 boundarySize = new Vector2(20f, 20f);
    [SerializeField] private float spacing = 2f;

    void Start()
    {
        CreateBoundary();
        CreateColliders();
    }

    void CreateBoundary()
    {
        // Left & Right
        for (float z = -boundarySize.y / 2; z <= boundarySize.y / 2; z += spacing)
        {
            SpawnMeteorid(new Vector3(-boundarySize.x / 2, 0, z));
            SpawnMeteorid(new Vector3(boundarySize.x / 2, 0, z));
        }

        // Top & Bottom
        for (float x = -boundarySize.x / 2; x <= boundarySize.x / 2; x += spacing)
        {
            SpawnMeteorid(new Vector3(x, 0, -boundarySize.y / 2));
            SpawnMeteorid(new Vector3(x, 0, boundarySize.y / 2));
        }
    }

    void SpawnMeteorid(Vector3 position)
    {
        int index = Random.Range(0, meteoridPrefabs.Length);
        Instantiate(meteoridPrefabs[index], position, Quaternion.identity, transform);
    }

    void CreateColliders()
    {
        // Create 4 walls as BoxColliders
        GameObject walls = new GameObject("BoundaryColliders");
        walls.transform.parent = transform;

        float thickness = 1f;

        // Left wall
        CreateWall(new Vector3(-boundarySize.x / 2 - thickness / 2, 0, 0),
                   new Vector3(thickness, 10, boundarySize.y));

        // Right wall
        CreateWall(new Vector3(boundarySize.x / 2 + thickness / 2, 0, 0),
                   new Vector3(thickness, 10, boundarySize.y));

        // Bottom wall
        CreateWall(new Vector3(0, 0, -boundarySize.y / 2 - thickness / 2),
                   new Vector3(boundarySize.x, 10, thickness));

        // Top wall
        CreateWall(new Vector3(0, 0, boundarySize.y / 2 + thickness / 2),
                   new Vector3(boundarySize.x, 10, thickness));
    }

    void CreateWall(Vector3 pos, Vector3 scale)
    {
        GameObject wall = new GameObject("Wall");
        wall.transform.parent = transform;
        wall.transform.localPosition = pos;
        wall.transform.localScale = scale;

        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.isTrigger = false; // set true if you only want to stop movement manually
    }
}
