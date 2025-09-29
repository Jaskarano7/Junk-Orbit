using UnityEngine;

public class AstronautAnimation1 : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpaceShipArea"))
        {
            animator.SetTrigger("ShouldFall");
        }
    }
   
}
