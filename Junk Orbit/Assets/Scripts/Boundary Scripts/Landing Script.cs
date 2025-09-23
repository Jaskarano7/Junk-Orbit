using UnityEngine;
using System.Collections;

public class LandingScript : MonoBehaviour
{
    [SerializeField] private PlayerDamageScript damageScript;
    [SerializeField] private PlayerJunkCollector junkCollector;
    [SerializeField] private UIManager uIManager;

    [SerializeField] private float TimeToLand = 5f;

    private bool hasLeftOnce = false;
    private bool isCounting = false;
    private Coroutine landingCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hasLeftOnce && !isCounting)
        {
            landingCoroutine = StartCoroutine(StartLandingCountdown());
            damageScript.isLanding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasLeftOnce)
            {
                hasLeftOnce = true;
            }

            if (landingCoroutine != null)
            {
                StopCoroutine(landingCoroutine);
                landingCoroutine = null;
                isCounting = false;
            }
            damageScript.isLanding = false ;
        }
    }

    private IEnumerator StartLandingCountdown()
    {
        isCounting = true;

        yield return new WaitForSeconds(TimeToLand);

        Debug.Log("Landing Completed");
        Debug.Log("Junk Collected: "+ junkCollector.JunkList.Count);
        uIManager.ShowLandingPage();
        isCounting = false;
        landingCoroutine = null;
    }
}
