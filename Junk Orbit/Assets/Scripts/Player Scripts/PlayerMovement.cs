using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Rigidbody PlayerRb;

    [Header("Variables")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Joystick")]
    [SerializeField] private Joystick joystick;

    [Header("Effects")]
    [SerializeField] private ParticleSystem rocketTrail;
    [SerializeField] private AudioSource rocketSound;
    [SerializeField] private float fadeSpeed = 2f;

    private float targetVolume = 0f;

    private void Start()
    {
        maxSpeed = 5;//playerData.PlayerSpeed;
        moveForce = 5;// playerData.PlayerAcceleration;
    }

    void FixedUpdate()
    {
        Movement();
        rocketSound.volume = Mathf.Lerp(rocketSound.volume, targetVolume, fadeSpeed * Time.fixedDeltaTime);
    }

    void Movement()
    {
        Vector2 input = joystick.Direction;
        Vector3 move = new Vector3(input.x, 0, input.y);

        if (move != Vector3.zero)
        {
            PlayerRb.AddForce(-move * moveForce, ForceMode.Acceleration);

            if (PlayerRb.linearVelocity.magnitude > maxSpeed)
            {
                PlayerRb.linearVelocity = PlayerRb.linearVelocity.normalized * maxSpeed;
            }

            // Rotate to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(-move, Vector3.up);
            Vector3 euler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(-90, 0, euler.y);
            PlayerRb.rotation = Quaternion.Slerp(PlayerRb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Effects
            if (!rocketTrail.isPlaying) rocketTrail.Play();
            targetVolume = 0.5f;
            if (!rocketSound.isPlaying) rocketSound.Play();
        }
        else
        {
            if (rocketTrail.isPlaying) rocketTrail.Stop();
            targetVolume = 0f;
            if (rocketSound.isPlaying && rocketSound.volume < 0.01f)
                rocketSound.Stop();
        }
    }

    public void UpdateSpeed(int newSpeed)
    {
        maxSpeed = newSpeed;
    }

    public void UpdateAcc(int newAcc)
    {
        moveForce = newAcc;
    }

    public void StopMovement()
    {
        rocketSound.Stop();
        this.enabled = false;
    }
}
