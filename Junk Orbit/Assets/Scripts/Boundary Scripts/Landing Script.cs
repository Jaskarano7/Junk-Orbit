using UnityEngine;
using System.Collections;

public class LandingScript : MonoBehaviour
{
    [SerializeField] private float TimeToLand = 5f;

    private bool hasLeftOnce = false;
    private bool isCounting = false;
    private Coroutine landingCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hasLeftOnce && !isCounting)
        {
            landingCoroutine = StartCoroutine(StartLandingCountdown());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasLeftOnce)
            {
                hasLeftOnce = true; // Mark that player has left once
                Debug.Log("Player left zone once, next entry will start landing check");
            }

            // If player leaves again during countdown, cancel landing
            if (landingCoroutine != null)
            {
                StopCoroutine(landingCoroutine);
                landingCoroutine = null;
                isCounting = false;
                Debug.Log("Player left before landing completed, countdown cancelled");
            }
        }
    }

    private IEnumerator StartLandingCountdown()
    {
        isCounting = true;
        Debug.Log("Player entered again, starting landing countdown...");

        yield return new WaitForSeconds(TimeToLand);

        Debug.Log("Landing Completed");
        isCounting = false;
        landingCoroutine = null;
    }
}
