using UnityEngine;

public class SpaceJunk : MonoBehaviour
{
    [Header("Junk Settings")]
    public JunkData junkInfo;

    [Header("Floating Settings")]
    [SerializeField] private float floatAmplitude = 0.5f;   // how far up/down it moves
    [SerializeField] private float floatFrequency = 1f;     // how fast it moves

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f;     // degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // default rotation axis

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;     // degrees per second

    private Vector3 startPos;
    private float randomOffset;

    private Transform player;
    private bool moveToPlayer = false;

    void Start()
    {
        FloatInitialize();
    }

    void Update()
    {
        if (moveToPlayer && player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            ContinueRotation();
        }
    }

    void FloatInitialize()
    {
        // Remember the initial position so it oscillates around it
        startPos = transform.position;

        // Add some randomness so not all scrap moves the same
        randomOffset = Random.Range(0f, 100f);
        floatAmplitude *= Random.Range(0.3f, 0.7f);
        floatFrequency *= Random.Range(0.8f, 1.2f);

        // Randomize rotation axis slightly
        rotationAxis = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)).normalized;
    }

    void ContinueRotation()
    {
        // Floating motion using sine wave
        float newY = startPos.y + Mathf.Sin((Time.time + randomOffset) * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Continuous slow rotation
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerJunkCollector playerJunk = other.GetComponent<PlayerJunkCollector>();
            PlayerData playerData = playerJunk.playerData;
            bool canCollect = junkInfo.Level <= playerData.PlayerLevel && playerJunk.currentCapacity + junkInfo.SpaceReq <= playerJunk.totalCapacity;
            if (canCollect)
            {
                player = other.transform;
                moveToPlayer = true;
            }
        }
    }

}

[System.Serializable]
public class JunkData
{
    public int Level = 1;
    public int SpaceReq = 1;
    public int Points = 1;
}