using UnityEngine;

public class InteractiveTriggerZone : MonoBehaviour
{
    public enum ActionType
    {
        SellJunk,
        UpgradeSpeed,
        UpgradeCapacity
    }

    [Header("Action Settings")]
    [SerializeField] private ActionType actionType;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PerformAction(other.gameObject);
        }
    }

    private void PerformAction(GameObject player)
    {
        switch (actionType)
        {
            case ActionType.SellJunk:
                Debug.Log("Selling junk for " + 10 + " coins.");
                break;

            case ActionType.UpgradeSpeed:
                break;

            case ActionType.UpgradeCapacity:
                break;
        }
    }
}
