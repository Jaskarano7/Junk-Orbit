using TMPro;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private AstronautAnimation astronautAnimationScript;
    [SerializeField] private GameObject UIBox;

    public void PlayAnimation()
    {
        astronautAnimationScript.StartMoving();
    }

    public void ActivePlayer()
    {
        astronautAnimationScript.ActivatePlayer();
    }

    public void RunFast()
    {
        astronautAnimationScript.RunFast();
    }

    public void NormalRun()
    {
        astronautAnimationScript.RunNormal();
    }

    public void ShowButtons()
    {
        UIBox.SetActive(true);
    }
}
