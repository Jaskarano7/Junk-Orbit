using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private GameObject HealthBarParent;
    [SerializeField] private Image HealthBarImage;
    
    [Header("Common Variables")]
    [SerializeField] private float fillSpeed = 2f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 5f;

    [Header("Points UI")]
    [SerializeField] private TextMeshProUGUI Points;

    private Vector3 originalPosition;
    private float HealthTargetFill;
    
    void Start()
    {
        HealthBarImage.fillAmount = 1f;
        HealthTargetFill = 1f;
        originalPosition = HealthBarParent.transform.localPosition;
    }
    void Update()
    {
        HealthBarImage.fillAmount = Mathf.MoveTowards(HealthBarImage.fillAmount, HealthTargetFill, Time.deltaTime * fillSpeed);
    }

    public void UpdateHealthBar(int currentHealth, int totalHealth)
    {
        HealthTargetFill = (float)currentHealth / totalHealth;
        if(HealthTargetFill > 0.66f)
        {
            HealthBarImage.color = Color.green;
        }
        else if(HealthTargetFill <= 0.66 && HealthTargetFill >= 0.33)
        {
            HealthBarImage.color = Color.yellow;
        }
        else
        {
            HealthBarImage.color = Color.red;
        }
    }
    public void SetHealthBarInstant(int currentHealth, int totalHealth)
    {
        float fill = (float)currentHealth / totalHealth;
        HealthTargetFill = fill;
        HealthBarImage.fillAmount = fill;
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

}
