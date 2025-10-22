using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator CameraAnimator;

    [Header("Animation Name")]
    [SerializeField] private string CameraAnimationName;

    [Header("Button")]
    [SerializeField] private Button UpgradeB;
    [SerializeField] private Button LaunchB;
    [SerializeField] private Button ShieldB;
    [SerializeField] private Button OxygenB;
    [SerializeField] private Button SpeedB;

    [Header("Rotation")]
    [SerializeField] private Transform SpaceShip;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float inertia = 5f;

    [Header("Button Control")]
    [SerializeField] private float[] targetRotations;

    private float rotationVelocity;
    private float currentTargetRotation;


    void Start()
    {
        InitalizeUpgradationButtons();
    }

    private void Update()
    {
        HandleButtonRotation();
    }

    void ToUpgradeStation()
    {
        CameraAnimator.Play(CameraAnimationName);
        LaunchB.gameObject.SetActive(false);
        UpgradeB.gameObject.SetActive(false);
    }

    void ToGamePage()
    {
        SceneManager.LoadScene(1);
    }

    private void HandleButtonRotation()
    {
        if (SpaceShip == null) return;

        float newY = Mathf.SmoothDampAngle(SpaceShip.eulerAngles.y, currentTargetRotation, ref rotationVelocity, 0.3f);
        SpaceShip.rotation = Quaternion.Euler(SpaceShip.eulerAngles.x, newY, SpaceShip.eulerAngles.z);

        if (Mathf.Abs(Mathf.DeltaAngle(SpaceShip.eulerAngles.y, currentTargetRotation)) < 0.1f)
        {
            rotationVelocity = 0;
        }
    }

    private void InitalizeUpgradationButtons()
    {
        UpgradeB.onClick.AddListener(ToUpgradeStation);
        LaunchB.onClick.AddListener(ToGamePage);
        ShieldB.onClick.AddListener(UpgradeShield);
        OxygenB.onClick.AddListener(UpgradeOxygen);
        SpeedB.onClick.AddListener(UpgradeSpeed);
    }

    private void RotateToTarget(int index)
    {
        if (index >= 0 && index < targetRotations.Length)
        {
            currentTargetRotation = targetRotations[index];

            rotationVelocity = 0;
        }
    }

    private void UpgradeShield()
    {
        RotateToTarget(0);
        if(GameSaver.instance.playerData.CurrentSheildLevel < 5 && PriceDecider(GameSaver.instance.playerData.CurrentSheildLevel+1) <= GameSaver.instance.playerData.PlayerPoints)
        {
            GameSaver.instance.playerData.CurrentSheildLevel += 1;
            GameSaver.instance.playerData.PlayerHealth += 1;
            GameSaver.instance.playerData.PlayerPoints -= PriceDecider(GameSaver.instance.playerData.CurrentSheildLevel);
            GameSaver.instance.playerData.DifficultyLevel = CheckCurrentLevel();
            Debug.Log(CheckCurrentLevel());
            GameSaver.instance.SavePlayerData();
        }
        else
        {
            Debug.Log("Already at max");
        }
    }
    private void UpgradeOxygen()
    {
        RotateToTarget(1);
        if (GameSaver.instance.playerData.CurrentOxygenLevel < 5 && PriceDecider(GameSaver.instance.playerData.CurrentOxygenLevel + 1) <= GameSaver.instance.playerData.PlayerPoints)
        {
            GameSaver.instance.playerData.CurrentOxygenLevel += 1;
            GameSaver.instance.playerData.PlayerOxygen += 2.5f;
            GameSaver.instance.playerData.PlayerPoints -= PriceDecider(GameSaver.instance.playerData.CurrentOxygenLevel);
            GameSaver.instance.playerData.DifficultyLevel = CheckCurrentLevel();
            Debug.Log(CheckCurrentLevel());
            GameSaver.instance.SavePlayerData();
        }
        else
        {
            Debug.Log("Already at max");
        }
    }
    private void UpgradeSpeed()
    {
        RotateToTarget(2);
        if (GameSaver.instance.playerData.CurrentSpeedLevel < 5 && PriceDecider(GameSaver.instance.playerData.CurrentSpeedLevel + 1) <= GameSaver.instance.playerData.PlayerPoints)
        {
            GameSaver.instance.playerData.CurrentSpeedLevel += 1;
            GameSaver.instance.playerData.PlayerSpeed += 2.5f;
            GameSaver.instance.playerData.PlayerAcceleration += 2.5f;
            GameSaver.instance.playerData.PlayerPoints -= PriceDecider(GameSaver.instance.playerData.CurrentSpeedLevel);
            GameSaver.instance.playerData.DifficultyLevel = CheckCurrentLevel();
            Debug.Log(CheckCurrentLevel());
            GameSaver.instance.SavePlayerData();
        }
        else
        {
            Debug.Log("Already at max");
        }
    }

    private int PriceDecider(int level)
    {
        switch (level)
        {
            case 1:
                return 50;
            case 2:
                return 50;
            case 3:
                return 250;
            case 4:
                return 500;
            case 5:
                return 1000;
            default:
                return 0;
        }
    }

    private int CheckCurrentLevel()
    {
        int level = GameSaver.instance.playerData.CurrentSheildLevel + GameSaver.instance.playerData.CurrentOxygenLevel + GameSaver.instance.playerData.CurrentSpeedLevel;
        if(level <= 5)
        {
            return 1;
        }
        else if (level >= 6 && level <= 8)
        {
            return 2;
        }
        else if(level >= 9 && level <= 12)
        {
            return 3;
        }
        else if (level > 12)
        {
            return 4;
        }
        else
        {
            return 0;
        }
    }
}
