using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject PlayerParent;
    [SerializeField] private GameObject Player;

    [Header("Variables")]
    [SerializeField] private float moveSpeed = 5f;   // adjust in Inspector
    [SerializeField] private float rotationSpeed = 10f; // smooth rotation

    [Header("Joystick")]
    [SerializeField] private Joystick joystick;

    [Header("Effects")]
    [SerializeField] private ParticleSystem rocketTrail; // drag your Particle System here
    [SerializeField] private AudioSource rocketSound;    // drag your AudioSource here
    [SerializeField] private float fadeSpeed = 2f;       // higher = faster fade

    private float targetVolume = 0f; // what volume we want

    void Update()
    {
        Movement();

        // Smoothly fade sound volume
        rocketSound.volume = Mathf.Lerp(rocketSound.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }

    void Movement()
    {
        Vector2 movement = joystick.Direction;
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        // Move the parent
        PlayerParent.transform.Translate(-move * moveSpeed * Time.deltaTime, Space.World);

        if (move != Vector3.zero)
        {
            // Rotation
            Quaternion targetRotation = Quaternion.LookRotation(-move, Vector3.up);
            Vector3 euler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(-90, 0, euler.y);
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (!rocketTrail.isPlaying) rocketTrail.Play();
            targetVolume = .5f;
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
}
