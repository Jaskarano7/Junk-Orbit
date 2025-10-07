using UnityEngine;
using UnityEngine.UI;

public class AstronautAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Child;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float RequiredRotattion = 60f;
    [SerializeField] private float rotationSpeed = 2f; // how fast to rotate

    public bool isMoving = false;
    private bool isFalling = false;
    private bool shouldRotate = false;
    private Quaternion targetRotation;


    public void StartMoving()
    {
        isMoving = true;
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

    private void RotatePlayer()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        targetRotation = Quaternion.Euler(currentRotation.x, RequiredRotattion, currentRotation.z);
        shouldRotate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpaceShipArea"))
        {
            Debug.Log("Triggered");
            animator.SetTrigger("ShouldFall");
            isFalling = true;
            speed = -5;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime,Space.World);
        }

        if (isFalling)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Falling"))
            {
                if (stateInfo.normalizedTime >= 0.25f)
                {
                    RotatePlayer();
                }

                if (stateInfo.normalizedTime >= 0.5f)
                {
                    StopMoving();
                }
                if(stateInfo.normalizedTime >= 0.9f)
                {
                    isFalling = false;
                }
            }
        }

        // Smooth rotation
        if (shouldRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Stop when close enough
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                shouldRotate = false;
            }
        }
    }
}
