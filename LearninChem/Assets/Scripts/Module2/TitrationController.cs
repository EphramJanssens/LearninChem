using UnityEngine;

public class TitrationController : MonoBehaviour
{
    public static TitrationController Instance;

    [Header("Visual Settings")]
    public Renderer liquidRenderer; 

    public Color startColor = new Color(0.9f, 0.9f, 0.9f, 0.4f); 
    
    public Color targetColor = new Color(1f, 0.1f, 0.6f, 0.9f); 
    
    public Color failedColor = new Color(0.5f, 0f, 0f, 1f);

    [Header("Timing (in seconds)")]
    public float timeToTargetColor = 4f; 
    public float timeToFail = 6f;

    private bool isTitrating = false;
    private float titrationTimer = 0f;
    private bool hasFailedLog = false;

    void Awake()
    {
        Instance = this;
        
        if (liquidRenderer == null)
        {
            Debug.LogWarning("<color=red>[TitrationController]</color> LET OP: liquidRenderer is leeg in de Inspector!");
        }
        else
        {
            Debug.Log("<color=green>[TitrationController]</color> Script is succesvol ingeladen en Cylinder is gekoppeld.");
        }
    }

    public void StartTitration()
    {
        Debug.Log("<color=cyan>[TitrationController]</color> StartTitration aangeroepen! De timer begint NU met lopen.");
        isTitrating = true;
        hasFailedLog = false;
    }

    public void StopTitration()
    {
        Debug.Log($"<color=cyan>[TitrationController]</color> StopTitration aangeroepen. Eindtijd: {titrationTimer} seconden.");
        isTitrating = false;
    }

    public void ResetLiquid()
    {
        isTitrating = false;
        titrationTimer = 0f;
        hasFailedLog = false;
        if (liquidRenderer != null)
        {
            liquidRenderer.material.color = startColor;
        }
    }

    void Update()
    {
        if (isTitrating)
        {
            titrationTimer += Time.deltaTime;

            if (titrationTimer <= timeToTargetColor)
            {
                float progress = titrationTimer / timeToTargetColor;
                liquidRenderer.material.color = Color.Lerp(startColor, targetColor, progress);
            }
            else if (titrationTimer <= timeToFail)
            {
                float failProgress = (titrationTimer - timeToTargetColor) / (timeToFail - timeToTargetColor);
                liquidRenderer.material.color = Color.Lerp(targetColor, failedColor, failProgress);
            }
            else
            {
                isTitrating = false;
                
                if (!hasFailedLog)
                {
                    Debug.Log("<color=red>[TitrationController]</color> MISLUKT! Kraan stond te lang open.");
                    hasFailedLog = true;
                }
                
                if (PPEDashboardTester.Instance != null)
                {
                    PPEDashboardTester.Instance.ShowFailure("Te ver getitreerd! Je hebt te veel zwavelzuur toegevoegd. Druk op Opnieuw Beginnen.");
                }
            }
        }
    }
}