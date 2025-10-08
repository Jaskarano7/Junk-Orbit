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

    [Header("Parent Objects")]
    [SerializeField] private GameObject UpgradationButtonParent;

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
        UpgradationButtonParent.SetActive(true);
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
        if(GameSaver.instance.playerData.CurrentSheildLevel < 5)
        {
            GameSaver.instance.playerData.CurrentSheildLevel += 1;
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
        if (GameSaver.instance.playerData.CurrentOxygenLevel < 5)
        {
            GameSaver.instance.playerData.CurrentOxygenLevel += 1;
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
        if (GameSaver.instance.playerData.CurrentSpeedLevel < 5)
        {
            GameSaver.instance.playerData.CurrentSpeedLevel += 1;
            GameSaver.instance.SavePlayerData();
        }
        else
        {
            Debug.Log("Already at max");
        }
    }
}
