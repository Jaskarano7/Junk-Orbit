using UnityEngine;

public class AstronautAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Child;
    [SerializeField] private float speed = 5f;

    public bool isMoving = false;

    public void StartMoving()
    {
        isMoving = true;
        Debug.Log("Player Running");
    }

    public void ActivatePlayer()
    {
        Child.SetActive(true);
    }

    public void RunFast()
    {
        speed = -12;
    }

    public void RunNormal()
    {
        speed = -10;
    }

    private void StopMoving()
    {
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpaceShipArea"))
        {
            animator.SetTrigger("ShouldFall");
            StopMoving();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
