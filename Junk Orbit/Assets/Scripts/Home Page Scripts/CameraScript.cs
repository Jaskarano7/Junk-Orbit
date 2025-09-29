using TMPro;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private AstronautAnimation astronautAnimationScript;

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

}
