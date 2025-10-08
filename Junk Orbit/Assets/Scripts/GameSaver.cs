using UnityEngine;

public class GameSaver : MonoBehaviour
{
    public static GameSaver instance;
    public PlayerData playerData;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.DeleteAll();
        LoadPlayerData();
    }

    // -----------------------
    // Save player data
    // -----------------------
    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("PlayerHealth", playerData.PlayerHealth);
        PlayerPrefs.SetInt("PlayerOxygen", playerData.PlayerOxygen);
        PlayerPrefs.SetInt("PlayerLevel", playerData.PlayerLevel);
        PlayerPrefs.SetInt("PlayerSpeed", playerData.PlayerSpeed);
        PlayerPrefs.SetInt("PlayerAcceleration", playerData.PlayerAcceleration);
        PlayerPrefs.SetInt("PlayerPoints", playerData.PlayerPoints);
        PlayerPrefs.SetInt("CurrentSheildLevel", playerData.CurrentSheildLevel);
        PlayerPrefs.SetInt("CurrentOxygenLevel", playerData.CurrentOxygenLevel);
        PlayerPrefs.SetInt("CurrentSpeedLevel", playerData.CurrentSpeedLevel);

        PlayerPrefs.Save();
        Debug.Log("Player data saved successfully!");
    }

    // -----------------------
    // Load player data
    // -----------------------
    public void LoadPlayerData()
    {
        // If no data saved yet, skip loading
        if (!PlayerPrefs.HasKey("PlayerHealth"))
        {
            Debug.Log("No save data found, initializing default values.");
            ResetToDefault();
            return;
        }

        playerData.PlayerHealth = PlayerPrefs.GetInt("PlayerHealth");
        playerData.PlayerOxygen = PlayerPrefs.GetInt("PlayerOxygen");
        playerData.PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");
        playerData.PlayerSpeed = PlayerPrefs.GetInt("PlayerSpeed");
        playerData.PlayerAcceleration = PlayerPrefs.GetInt("PlayerAcceleration");
        playerData.PlayerPoints = PlayerPrefs.GetInt("PlayerPoints");
        playerData.CurrentSheildLevel = PlayerPrefs.GetInt("CurrentSheildLevel");
        playerData.CurrentOxygenLevel = PlayerPrefs.GetInt("CurrentOxygenLevel");
        playerData.CurrentSpeedLevel = PlayerPrefs.GetInt("CurrentSpeedLevel");

        Debug.Log("Player data loaded successfully!");
    }

    // -----------------------
    // Reset to default
    // -----------------------
    public void ResetToDefault()
    {
        playerData.PlayerHealth = 5;
        playerData.PlayerOxygen = 15;
        playerData.PlayerLevel = 1;
        playerData.PlayerSpeed = 5;
        playerData.PlayerAcceleration = 5;
        playerData.PlayerPoints = 0;
        playerData.CurrentSheildLevel = 1;
        playerData.CurrentOxygenLevel = 1;
        playerData.CurrentSpeedLevel = 1;
}
}
