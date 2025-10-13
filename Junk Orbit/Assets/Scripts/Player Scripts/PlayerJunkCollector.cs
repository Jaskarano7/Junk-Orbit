using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerJunkCollector : MonoBehaviour
{
    [Header("Script Ref")]
    public PlayerData playerData;
    public UIManager uIManager;

    [Header("Collect Settings")]
    public List<JunkData> JunkList = new List<JunkData>();

    [Header("Collect Settings")]
    [SerializeField] private float shrinkDuration = 0.3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip Collect;
    [SerializeField] private AudioClip Full;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Junk"))
        {
            GameObject junk = other.gameObject.transform.parent.gameObject;
            SpaceJunk spaceJunk = junk.GetComponent<SpaceJunk>();

            if (spaceJunk == null)
            {
                Debug.LogError($"{junk.name} does not have a SpaceJunk component!");
                return;
            }

            StartCoroutine(ShrinkAndDestroy(junk, spaceJunk));
        }
    }

    private IEnumerator ShrinkAndDestroy(GameObject junk, SpaceJunk spaceJunk)
    {
        if (junk == null) yield break;

        Vector3 originalScale = junk.transform.localScale;
        float time = 0f;

        if (audioSource != null && Collect != null)
            audioSource.PlayOneShot(Collect);

        while (time < shrinkDuration)
        {
            if (junk == null) yield break; // safety check

            float t = time / shrinkDuration;
            junk.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (junk != null) // final check before accessing
        {
            junk.transform.localScale = Vector3.zero;

            if (spaceJunk != null)
                JunkList.Add(spaceJunk.junkInfo);

            Destroy(junk);
        }
    }
}
