using UnityEngine;

public class SpaceJunk : MonoBehaviour
{
    [Header("Junk Settings")]
    public JunkData junkInfo;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;     // degrees per second

    private Transform player;
    private bool moveToPlayer = false;

    
    void Update()
    {
        if (moveToPlayer && player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerJunkCollector playerJunk = other.GetComponent<PlayerJunkCollector>();
            PlayerData playerData = playerJunk.playerData;
            player = other.transform;
            moveToPlayer = true;
        }
    }

}

[System.Serializable]
public class JunkData
{
    public string Name;
    public int Level = 1;
    public int Points = 1;
}