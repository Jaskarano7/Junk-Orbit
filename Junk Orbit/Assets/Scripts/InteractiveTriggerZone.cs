using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class InteractiveTriggerZone : MonoBehaviour
{
    [Header("Action Settings")]
    [SerializeField] private ActionType actionType;

    [Header("Script Reference")]
    [SerializeField] private PlayerJunkCollector collector;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private UIManager uIManager;

    [Header("Script Reference")]
    [SerializeField] private AudioSource audioSource;

    private Coroutine sellCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PerformAction(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sellCoroutine != null)
            {
                StopCoroutine(sellCoroutine);
                sellCoroutine = null;
            }
        }
    }

    private void PerformAction(GameObject player)
    {
        switch (actionType)
        {
            case ActionType.SellJunk:
                if (sellCoroutine == null && collector.JunkList.Count > 0)
                    sellCoroutine = StartCoroutine(SellJunk());
                break;

            case ActionType.UpgradeSpeed:
                SpeedUpgradation(3);
                break;

            case ActionType.UpgradeAcceleration:
                break;

            case ActionType.UpgradeCapacity:
                CapacityUpgradation(3);
                break;
        }
    }

    private IEnumerator SellJunk()
    {
        while (collector.JunkList.Count > 0)
        {
            var junk = collector.JunkList[0];
            playerData.PlayerPoints += junk.Points;
            collector.currentCapacity -= junk.SpaceReq;

            uIManager.SetCapacityBarInstant(collector.currentCapacity, collector.totalCapacity);
            uIManager.UpdatePoints(playerData.PlayerPoints);
            audioSource.Play();
            collector.JunkList.RemoveAt(0);

            yield return new WaitForSeconds(.2f);
        }

        sellCoroutine = null;
    }

    void SpeedUpgradation(int UpgradePrice)
    {
        if(UpgradePrice <= playerData.PlayerPoints)
        {
            // Increase the speed
            playerData.PlayerSpeed += 10;
            playerMovement.UpdateSpeed(playerData.PlayerSpeed);

            //Deduct the money
            playerData.PlayerPoints -= UpgradePrice;
            uIManager.UpdatePoints(playerData.PlayerPoints);

            Debug.Log("Upgrade Succesfull : Speed");
        }
        else
        {
            Debug.Log("Less money");
        }
    }

    void CapacityUpgradation(int UpgradePrice)
    {
        if (UpgradePrice <= playerData.PlayerPoints)
        {
            // Increase the speed
            playerData.PlayerCapacity += 5;
            collector.UpdateCapacity(playerData.PlayerCapacity);

            //Deduct the money
            playerData.PlayerPoints -= UpgradePrice;
            uIManager.UpdatePoints(playerData.PlayerPoints);

            Debug.Log("Upgrade Succesfull : Capacity");
        }
        else
        {
            Debug.Log("Less money");
        }
    }
}

public enum ActionType
{
    SellJunk,
    UpgradeSpeed,
    UpgradeAcceleration,
    UpgradeCapacity
}
