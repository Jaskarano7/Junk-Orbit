using UnityEngine;
using System.Collections;

public class PlayerJunkCollector : MonoBehaviour
{
    [Header("Script Ref")]
    public PlayerData playerData;
    public UIManager uIManager;

    [Header("Collect Settings")]
    [SerializeField] private float shrinkDuration = 0.3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip Collect;
    [SerializeField] private AudioClip Full;

    public int currentCapacity;
    public int totalCapacity;

    private void Start()
    {
        totalCapacity = playerData.PlayerCapacity;
        currentCapacity = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Junk"))
        {
            GameObject junk = other.gameObject.transform.root.gameObject;
            SpaceJunk spaceJunk = junk.GetComponent<SpaceJunk>();

            // Check: player level high enough AND enough space
            bool hasCapacity = currentCapacity + spaceJunk.SpaceReq <= totalCapacity;
            bool isLevelMatch = spaceJunk.Level <= playerData.PlayerLevel;

            if (hasCapacity && isLevelMatch)
            {
                currentCapacity += spaceJunk.SpaceReq;
                StartCoroutine(ShrinkAndDestroy(junk));
            }
            else
            {
                if (!hasCapacity)
                {
                    Debug.Log("Cannot collect: Not enough space");
                }
                if (!isLevelMatch)
                {
                    Debug.Log("Cannot collect: Level too low");
                }
                uIManager.ShakeBar();
                audioSource.PlayOneShot(Full);
            }
        }
    }

    private IEnumerator ShrinkAndDestroy(GameObject junk)
    {
        Vector3 originalScale = junk.transform.localScale;
        float time = 0f;

        audioSource.PlayOneShot(Collect);

        while (time < shrinkDuration)
        {
            float t = time / shrinkDuration;
            junk.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            time += Time.deltaTime;
            yield return null;
        }

        junk.transform.localScale = Vector3.zero;
        uIManager.UpdateCapacityBar(currentCapacity,totalCapacity);
        Destroy(junk);
    }
}
