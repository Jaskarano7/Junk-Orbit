using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator CameraAnimator;

    [Header("Animation Name")]
    [SerializeField] private string CameraAnimationName;

    [Header("Buttons")]
    [SerializeField] private Button UpgradeB;
    [SerializeField] private Button LaunchB;
    [SerializeField] private Button ShieldB;
    [SerializeField] private Button OxygenB;
    [SerializeField] private Button SpeedB;

    [Header("Rotation")]
    [SerializeField] private Transform SpaceShip;
    [SerializeField] private float rotationSmooth = 0.3f;
    [SerializeField] private float[] targetRotations;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI HeadingText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip UpgradationSound;

    [Header("Upgrades")]
    [SerializeField] private UpgradeType[] upgrades;

    private float rotationVelocity;
    private float currentTargetRotation;

    void Start()
    {
        InitializeButtons();
    }

    void Update()
    {
        RotateShipSmoothly();
    }

    // ------------------- NAVIGATION -------------------

    void ToUpgradeStation()
    {
        CameraAnimator.Play(CameraAnimationName);

        LaunchB.gameObject.SetActive(false);
        UpgradeB.gameObject.SetActive(false);
        HeadingText.gameObject.SetActive(false);

        RefreshAllPrices();
    }

    void ToGamePage()
    {
        SceneManager.LoadScene(1);
    }

    // ------------------- BUTTON SETUP -------------------

    private void InitializeButtons()
    {
        UpgradeB.onClick.AddListener(ToUpgradeStation);
        LaunchB.onClick.AddListener(ToGamePage);

        ShieldB.onClick.AddListener(() => Upgrade(0));
        OxygenB.onClick.AddListener(() => Upgrade(1));
        SpeedB.onClick.AddListener(() => Upgrade(2));
    }

    // ------------------- ROTATION -------------------

    private void RotateShipSmoothly()
    {
        if (!SpaceShip) return;

        float newY = Mathf.SmoothDampAngle(
            SpaceShip.eulerAngles.y,
            currentTargetRotation,
            ref rotationVelocity,
            rotationSmooth
        );

        SpaceShip.rotation = Quaternion.Euler(
            SpaceShip.eulerAngles.x,
            newY,
            SpaceShip.eulerAngles.z
        );

        if (Mathf.Abs(Mathf.DeltaAngle(SpaceShip.eulerAngles.y, currentTargetRotation)) < 0.1f)
            rotationVelocity = 0;
    }

    private void RotateToTarget(int index)
    {
        if (index >= 0 && index < targetRotations.Length)
        {
            currentTargetRotation = targetRotations[index];
            rotationVelocity = 0;
        }
    }

    // ------------------- UPGRADING -------------------

    private void Upgrade(int index)
    {
        var data = GameSaver.instance.playerData;
        UpgradeType u = upgrades[index];

        RotateToTarget(index);

        if (u.currentLevel >= u.maxLevel)
        {
            Debug.Log("Already at max level!");
            u.priceText.text = "MAX";
            return;
        }

        int nextLevel = u.currentLevel + 1;
        int cost = u.GetPrice(nextLevel);

        if (data.PlayerPoints < cost)
        {
            Debug.Log($"Insufficient funds. Need {cost}");
            return;
        }

        // Deduct points
        data.PlayerPoints -= cost;

        // Level up
        u.currentLevel++;

        // Apply stats
        ApplyStats(u);

        // Update UI (MAX if reached)
        UpdatePriceText(u);

        // Difficulty update
        data.DifficultyLevel = CalculateDifficulty();

        // Save data
        GameSaver.instance.SavePlayerData();

        // Sound
        audioSource.PlayOneShot(UpgradationSound);
    }

    private void ApplyStats(UpgradeType u)
    {
        var data = GameSaver.instance.playerData;
        switch (u.statType)
        {
            case UpgradeType.UpgradeStatType.Health:
                data.PlayerHealth += u.statIncrease;
                data.CurrentSheildLevel += 1;
                break;

            case UpgradeType.UpgradeStatType.Oxygen:
                data.PlayerOxygen += u.statIncrease;
                data.CurrentOxygenLevel += 1;
                break;

            case UpgradeType.UpgradeStatType.Speed:
                data.PlayerSpeed += u.statIncrease;
                data.PlayerAcceleration += u.statIncrease;
                data.CurrentSpeedLevel += 1;
                break;
        }
        //GameSaver.instance.SavePlayerData();
    }

    // ------------------- UI UPDATE -------------------

    private void RefreshAllPrices()
    {
        var data = GameSaver.instance.playerData;

        UpdatePriceText(upgrades[0], data.CurrentSheildLevel);
        UpdatePriceText(upgrades[1], data.CurrentOxygenLevel);
        UpdatePriceText(upgrades[2], data.CurrentSpeedLevel);
    }

    private void UpdatePriceText(UpgradeType u, int levelOverride = -1)
    {
        int level = (levelOverride == -1) ? u.currentLevel : levelOverride;

        if (level >= u.maxLevel)
        {
            u.priceText.text = "MAX";
            return;
        }

        u.priceText.text = u.GetPrice(level + 1).ToString();
    }

    // ------------------- DIFFICULTY -------------------

    private int CalculateDifficulty()
    {
        var d = GameSaver.instance.playerData;

        int sum = d.CurrentSheildLevel + d.CurrentOxygenLevel + d.CurrentSpeedLevel;

        if (sum <= 5) return 1;
        if (sum <= 8) return 2;
        if (sum <= 12) return 3;
        return 4;
    }
}
