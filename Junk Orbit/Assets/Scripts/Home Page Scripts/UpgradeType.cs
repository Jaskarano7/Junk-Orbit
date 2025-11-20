using TMPro;
using UnityEngine;

[System.Serializable]
public class UpgradeType
{
    public string name;

    public enum UpgradeStatType { Health, Oxygen, Speed }
    public UpgradeStatType statType;

    public int currentLevel;
    public int maxLevel = 5;

    public float statIncrease;

    public TextMeshProUGUI priceText;

    // Centralized price logic
    public int GetPrice(int nextLevel)
    {
        switch (nextLevel)
        {
            case 1: return 50;
            case 2: return 50;
            case 3: return 250;
            case 4: return 500;
            case 5: return 1000;
        }
        return 0;
    }
}
