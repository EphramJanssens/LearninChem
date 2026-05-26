using UnityEngine;
using UnityEngine.UIElements;

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
        finalValue = 0; // FIX: Wis de gemeten waarde uit het geheugen!
        if (displayLabel != null) displayLabel.text = "--- µS/cm";
    }
}