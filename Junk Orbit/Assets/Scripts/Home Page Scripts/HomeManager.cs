using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator CameraAnimator;

    [Header("Animation Name")]
    [SerializeField] private string CameraAnimationName;

    [Header("Button")]
    [SerializeField] private Button UpgradeB;

    void Start()
    {
        UpgradeB.onClick.AddListener(toUpgradeStation);
    }


    void toUpgradeStation()
    {
        CameraAnimator.Play(CameraAnimationName);
    }
}
