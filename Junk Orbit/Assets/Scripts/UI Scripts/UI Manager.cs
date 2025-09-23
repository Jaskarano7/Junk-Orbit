using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    
    [Header("Health UI")]
    [SerializeField] private GameObject HealthBarParent;
    [SerializeField] private Image HealthBarImage;

    [Header("Oxygen UI")]
    [SerializeField] private Image OxygenBarImage; 

    [Header("Common Variables")]
    [SerializeField] private float fillSpeed = 2f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 5f;

    [Header("Page UI")]
    [SerializeField] private GameObject Page;
    [SerializeField] private TextMeshProUGUI PageT;
    [SerializeField] private GameObject Joystick;
    [SerializeField] private Button Home;

    private Vector3 originalPosition;
    private float HealthTargetFill;
    private float OxygenTargetFill;
    
    void Start()
    {
        HealthBarImage.fillAmount = 1f;
        OxygenBarImage.fillAmount = 1f;
        HealthTargetFill = 1f;
        OxygenTargetFill = 1f;
        originalPosition = HealthBarParent.transform.localPosition;

        // Page
        Home.onClick.AddListener(GoHome);
    }
    void Update()
    {
        HealthBarImage.fillAmount = Mathf.MoveTowards(HealthBarImage.fillAmount, HealthTargetFill, Time.deltaTime * fillSpeed);
        OxygenBarImage.fillAmount = Mathf.MoveTowards(OxygenBarImage.fillAmount, OxygenTargetFill, Time.deltaTime * fillSpeed);
    }

    public void UpdateHealthBar(float currentHealth, float totalHealth)
    {
        HealthTargetFill = (float)currentHealth / totalHealth;
        if (HealthTargetFill > 0.66f)
        {
            HealthBarImage.color = Color.green;
        }
        else if (HealthTargetFill <= 0.66 && HealthTargetFill >= 0.33)
        {
            HealthBarImage.color = Color.yellow;
        }
        else
        {
            HealthBarImage.color = Color.red;
        }
    }
    public void UpdateOxygenBar(float currentOxygen, float totalOxygen)
    {
        OxygenTargetFill = (float)currentOxygen / totalOxygen;
    }

    public void ShakeBar()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float yOffset = Mathf.Sin(elapsed * 40f) * shakeStrength; // up-down motion
            HealthBarParent.transform.localPosition = originalPosition + new Vector3(0, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // reset position
        HealthBarParent.transform.localPosition = originalPosition;
    }


    #region

    void GoHome()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowDeadPage()
    {
        Page.SetActive(true);
        Joystick.SetActive(false);
        playerMovement.StopMovement();
    }

    public void ShowLandingPage()
    {
        PageT.text = "Landing Completed";
        Page.SetActive(true);
        Joystick.SetActive(false);
        playerMovement.StopMovement();
    }

    #endregion

}
