using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Capacity UI")]
    [SerializeField] private GameObject CapacityBarParent;
    [SerializeField] private Image CapacityBarImage;
    [SerializeField] private float fillSpeed = 2f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 5f;

    private float targetFill;
    private Vector3 originalPosition;

    [Header("Capacity UI")]
    [SerializeField] private TextMeshProUGUI Points;

    void Start()
    {
        CapacityBarImage.fillAmount = 0f;
        targetFill = 0f;
        if (CapacityBarParent != null)
            originalPosition = CapacityBarParent.transform.localPosition;
    }
    void Update()
    {
        CapacityBarImage.fillAmount = Mathf.MoveTowards(CapacityBarImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);
    }

    public void UpdateCapacityBar(int currentCapacity, int totalCapacity)
    {
        targetFill = (float)currentCapacity / totalCapacity;
    }
    public void SetCapacityBarInstant(int currentCapacity, int totalCapacity)
    {
        float fill = (float)currentCapacity / totalCapacity;
        targetFill = fill;
        CapacityBarImage.fillAmount = fill;
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
            CapacityBarParent.transform.localPosition = originalPosition + new Vector3(0, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // reset position
        CapacityBarParent.transform.localPosition = originalPosition;
    }

    public void UpdatePoints(int points)
    {
        Points.text = "P: " + points;
    }
}
