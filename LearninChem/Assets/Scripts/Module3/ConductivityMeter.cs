using UnityEngine;
using UnityEngine.UIElements;

public class ConductivityMeter : MonoBehaviour
{
    public static ConductivityMeter Instance;

    [Header("UI Display")]
    public UIDocument meterUI;
    private Label displayLabel;

    [Header("Instellingen")]
    public string targetTag = "StirZone"; // We kunnen de StirZone uit de beker als water-detector gebruiken!
    public float measurementDuration = 3.0f;
    public int minConductivity = 390;
    public int maxConductivity = 430;

    private bool isMeasuring = false;
    private bool hasMeasured = false;
    private float timer = 0f;
    
    // Deze waarde halen we straks op met het Dashboard
    public int finalValue { get; private set; } 

    void Awake()
    {
        Instance = this;
        if (meterUI != null && meterUI.rootVisualElement != null)
        {
            // DE FIX: Verander de zoekterm naar jouw label-naam
            displayLabel = meterUI.rootVisualElement.Q<Label>("ConductivityText");
            
            if (displayLabel != null)
            {
                displayLabel.text = "--- µS/cm";
            }
            else
            {
                // Extra vangnet voor de toekomst
                Debug.LogError("<color=red>[ConductivityMeter]</color> Label 'ConductivityText' niet gevonden in de UXML!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Alleen starten als we nog niet gemeten hebben en de zone de juiste tag heeft
        if (hasMeasured || isMeasuring) return;

        if (other.CompareTag(targetTag))
        {
            StartMeasurement();
        }
    }

    private void StartMeasurement()
    {
        isMeasuring = true;
        timer = 0f;
        
        // Genereer de geheime eindwaarde (bijv. 412)
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
                // Fluctueer wild rondom de eindwaarde (simuleert het zoeken naar een stabiele meting)
                int randomFluctuation = finalValue + Random.Range(-25, 25);
                if (displayLabel != null) displayLabel.text = $"{randomFluctuation} µS/cm";
            }
            else
            {
                // De 3 seconden zijn om! Zet de definitieve waarde vast.
                isMeasuring = false;
                hasMeasured = true;
                
                if (displayLabel != null) displayLabel.text = $"{finalValue} µS/cm";
                
                Debug.Log("<color=green>[ConductivityMeter]</color> Meting gestabiliseerd!");
                
                // Vertel de manager dat de elektrode correct is gebruikt
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
        if (displayLabel != null) displayLabel.text = "--- µS/cm";
    }
}