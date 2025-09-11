using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerJunkCollector : MonoBehaviour
{
    [Header("Script Ref")]
    public PlayerData playerData;
    public UIManager uIManager;

    [Header("Collect Settings")]
    public List<JunkData> JunkList;

    [Header("Collect Settings")]
    [SerializeField] private float shrinkDuration = 0.3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip Collect;
    [SerializeField] private AudioClip Full;


    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Junk"))
        {
            GameObject junk = other.gameObject.transform.root.gameObject;
            SpaceJunk spaceJunk = junk.GetComponent<SpaceJunk>();

            StartCoroutine(ShrinkAndDestroy(junk, spaceJunk));
        }
    }

    private IEnumerator ShrinkAndDestroy(GameObject junk, SpaceJunk spaceJunk)
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
        JunkList.Add(spaceJunk.junkInfo);

        Destroy(junk);
    }
}
