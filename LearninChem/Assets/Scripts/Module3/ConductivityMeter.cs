using UnityEngine;
using UnityEngine.UIElements;

/*
 * Functie: Simuleert een geleidbaarheidsmeting (met realistische fluctuaties en een willekeurige eindwaarde) zodra de elektrode de vloeistof raakt. Controleert of de oplossing eerst is geroerd.
 * Invloed: Voorziet de DashboardInputValidator van de juiste eindwaarde en stuurt het succes signaal ("InsertProbe") of foutmeldingen door naar de UniversalProcedureManager.
 */

public class ConductivityMeter : MonoBehaviour
{
    public static ConductivityMeter Instance;

    [Header("UI Display")]
    public UIDocument meterUI;
    private Label displayLabel;

    [Header("Instellingen")]
    public string targetTag = "StirZone"; 
    public float measurementDuration = 3.0f;
    public int minConductivity = 390;
    public int maxConductivity = 430;

    private bool isMeasuring = false;
    private bool hasMeasured = false;
    private float timer = 0f;
    
    public bool isSolutionStirred = false; 
    
    public int finalValue { get; private set; } 

    void Awake()
    {
        Instance = this;
        if (meterUI != null && meterUI.rootVisualElement != null)
        {
            displayLabel = meterUI.rootVisualElement.Q<Label>("ConductivityText");
            
            if (displayLabel != null)
            {
                displayLabel.text = "--- µS/cm";
            }
            else
            {
                Debug.LogError("<color=red>[ConductivityMeter]</color> Label 'ConductivityText' niet gevonden in de UXML!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasMeasured || isMeasuring) return;

        if (other.CompareTag(targetTag))
        {
            if (!isSolutionStirred)
            {
                Debug.LogWarning("<color=orange>[ConductivityMeter]</color> Meting geblokkeerd: Oplossing is nog niet geroerd!");
                
                if (UniversalProcedureManager.Instance != null)
                {
                    UniversalProcedureManager.Instance.ShowGlobalFailure("Fout! Je moet de oplossing eerst goed roeren voordat je een betrouwbare meting kan doen.");
                }
                return;
            }

            StartMeasurement();
        }
    }

    private void StartMeasurement()
    {
        isMeasuring = true;
        timer = 0f;
        
        finalValue = Random.Range(minConductivity, maxConductivity + 1);
        Debug.Log($"<color=cyan>[ConductivityMeter]</color> Elektrode in vloeistof! Doelwaarde wordt: {finalValue}");
    }

    void Update()
    {
        if (isMeasuring)
        {
            timer += Time.deltaTime;

            if (timer < measurementDuration)
            {
                int randomFluctuation = finalValue + Random.Range(-25, 25);
                if (displayLabel != null) displayLabel.text = $"{randomFluctuation} µS/cm";
            }
            else
            {
                isMeasuring = false;
                hasMeasured = true;
                
                if (displayLabel != null) displayLabel.text = $"{finalValue} µS/cm";
                
                Debug.Log("<color=green>[ConductivityMeter]</color> Meting gestabiliseerd!");
                
                if (UniversalProcedureManager.Instance != null)
                {
                    UniversalProcedureManager.Instance.OnActionTriggered("InsertProbe");
                }
            }
        }
    }

    public void ResetMeter()
    {
        isMeasuring = false;
        hasMeasured = false;
        timer = 0f;
        finalValue = 0;
        
        isSolutionStirred = false; 
        
        if (displayLabel != null) displayLabel.text = "--- µS/cm";
    }
}