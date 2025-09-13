using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageScript : MonoBehaviour
{
    [Header("Script Reference")]
    [SerializeField] private CameraFollow cameraF;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private UIManager uIManager;

    [Header("Health")]
    [SerializeField] private float CurrentHealth;
    [SerializeField] private float TotalHealth;

    [Header("Oxygen")]
    [SerializeField] private float CurrentOxygen;
    [SerializeField] private float TotalOxygen;

    [Header("Damage Overlay")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private float flashDuration = 0.3f;
    [SerializeField] private float maxAlpha = 0.5f;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 8f;   // strength of push
    [SerializeField] private float knockbackUpward = 2f;  // little vertical bump
    [SerializeField] private bool stunOnHit = false;      // toggle stun
    [SerializeField] private float stunDuration = 1f;     // how long player is stunned

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ThudSound;

    [Header("Cooldown")]
    [SerializeField] private float hitCooldown = 0.5f;    // delay before next hit

    private Coroutine flashRoutine;
    private Rigidbody rb;
    private bool isStunned = false;
    private bool recentlyHit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CurrentHealth = TotalHealth = playerData.PlayerHealth;
        CurrentOxygen = TotalOxygen= playerData.PlayerOxygen;

        StartCoroutine(ReduceOxygen());
    }

    // --- Overlay flash ---
    public void ShowDamageScreen()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        uIManager.ShakeBar();
        flashRoutine = StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        float elapsed = 0f;

        // fade in
        while (elapsed < flashDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, maxAlpha, elapsed / (flashDuration / 2f));
            overlayImage.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        // fade out
        elapsed = 0f;
        while (elapsed < flashDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0, elapsed / (flashDuration / 2f));
            overlayImage.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        overlayImage.color = new Color(1f, 0f, 0f, 0f);
        flashRoutine = null;
    }

    // --- Stun logic ---
    private IEnumerator StunPlayer()
    {
        isStunned = true;
        Debug.Log("Player stunned!");

        // Disable movement script if present
        var moveScript = GetComponent<PlayerMovement>(); // replace with your script
        if (moveScript != null)
            moveScript.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        if (moveScript != null)
            moveScript.enabled = true;

        Debug.Log("Player recovered from stun.");
        isStunned = false;
    }

    // --- Collision ---
    private void OnCollisionEnter(Collision collision)
    {
        if (recentlyHit) return; // cooldown check

        if (collision.collider.CompareTag("Obstacle"))
        {
            Debug.Log("Collided damage received");

            if (cameraF != null)
            {
                cameraF.ShakeCamera();
            }
            ShowDamageScreen();

            //Damge Updation
            CurrentHealth -= 1;
            CheckOfGameOver();
            uIManager.UpdateHealthBar(CurrentHealth, TotalHealth);

            // play sound
            if (audioSource != null && ThudSound != null)
            {
                audioSource.PlayOneShot(ThudSound);
            }

            // knockback
            if (rb != null)
            {
                Vector3 knockDir = (transform.position - collision.transform.position).normalized;
                knockDir.y = 0f;
                rb.AddForce(knockDir * knockbackForce + Vector3.up * knockbackUpward, ForceMode.Impulse);
            }

            if (stunOnHit && !isStunned)
            {
                StartCoroutine(StunPlayer());
            }

            StartCoroutine(HitCooldown());
        }
    }

    private IEnumerator HitCooldown()
    {
        recentlyHit = true;
        yield return new WaitForSeconds(hitCooldown);
        recentlyHit = false;
    }

    // --- Game Over -- 
    void CheckOfGameOver()
    {
        if (CurrentHealth <= 0)
        {
            Debug.Log("You are dead");
        }
    }

    IEnumerator ReduceOxygen()
    {
        float timeWithoutOxygen = 0f;

        while (true)
        {
            if (CurrentOxygen > 0)
            {
                timeWithoutOxygen = 0f;

                CurrentOxygen -= 1;
                uIManager.UpdateOxygenBar(CurrentOxygen, TotalOxygen);
            }
            else
            {
                timeWithoutOxygen += 1f;
                float damageAmount;
                
                if (timeWithoutOxygen <= 5f)
                {
                    damageAmount = 0.1f;
                }
                else if (timeWithoutOxygen <= 10f)
                {
                    damageAmount = 0.5f;
                }
                else
                {
                    damageAmount = 1f;
                }

                CurrentHealth -= damageAmount;
                uIManager.UpdateHealthBar(CurrentHealth, TotalHealth);
            }

            if (CurrentHealth <= 0)
            {
                Debug.Log("GameOver");
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
