using UnityEngine;

/*
 * Functie: Beheert de timer en de visuele kleuromslag (Lerp) van de vloeistof tijdens de titratie, en controleert op overtitratie (fail-state).
 * Invloed: Communiceert met de UniversalProcedureManager bij een mislukking. Wordt aangestuurd door de BuretValve (Start/Stop) en LiquidTriggerZone (voorbereiding).
 */

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
    
    public bool isFailed = false;
    public bool isBeakerPrepared = false; 

/*
* Zorgt dat dit script overal bereikbaar is (Singleton) en verifieert of het 3D materiaal van de vloeistof succesvol is ingeladen.
*/
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

/*
* Controleert eerst of de beker voorbereid is (indicator toegevoegd), start de timer en haalt de vloeistof uit een eventuele fail state.
*/
    public void StartTitration()
    {
        if (!isBeakerPrepared)
        {
            Debug.LogWarning("<color=orange>[TitrationController]</color> Beker is nog niet voorbereid! De kleur zal niet veranderen.");
            return;
        }

        Debug.Log("<color=cyan>[TitrationController]</color> StartTitration aangeroepen! De timer begint NU met lopen.");
        isTitrating = true;
        isFailed = false;
    }

/*
* Pauzeert de timer wanneer de speler de kraan van de buret dichtdraait.
*/
    public void StopTitration()
    {
        Debug.Log($"<color=cyan>[TitrationController]</color> StopTitration aangeroepen. Eindtijd: {titrationTimer} seconden.");
        isTitrating = false;
    }

/*
* Reset de tijdsmeting, fail states en herstelt de transparante startkleur van het materiaal.
*/
    public void ResetLiquid()
    {
        isTitrating = false;
        titrationTimer = 0f;
        isFailed = false;
        
        isBeakerPrepared = false; 
        
        if (liquidRenderer != null)
        {
            liquidRenderer.material.color = startColor;
        }
    }

/*
* Voert elke frame controles uit als de kraan open staat. Berekent de vloeiende kleurovergang (Lerp) op basis van tijd, en triggert de overtitratie fout als de limiet wordt overschreden.
*/
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
                
                if (!isFailed)
                {
                    Debug.Log("<color=red>[TitrationController]</color> MISLUKT! Kraan stond te lang open.");
                    isFailed = true;
                }
                
                if (UniversalProcedureManager.Instance != null)
                {
                    UniversalProcedureManager.Instance.ShowGlobalFailure("Te ver getitreerd! Je hebt te veel zwavelzuur toegevoegd. Keer terug naar het hoofdmenu om opnieuw te beginnen.");
                }
            }
        }
    }
}