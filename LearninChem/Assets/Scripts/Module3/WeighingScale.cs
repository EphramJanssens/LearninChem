using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeighingScale : MonoBehaviour
{
    [Header("UI & Knoppen")]
    public UIDocument scaleUI;
    public XRSimpleInteractable tareButton; 

    [Header("Gewichten (in gram)")]
    public float emptyBeakerWeight = 45.20f;
    public float targetKalkWeight = 5.50f;

    private float currentWeight = 0.00f;
    private bool hasBeaker = false;
    private bool isTared = false;
    
    private Label displayLabel;

    void Awake()
    {
        if (scaleUI != null && scaleUI.rootVisualElement != null)
        {
            displayLabel = scaleUI.rootVisualElement.Q<Label>("WeightDisplay");
        }

        if (tareButton != null)
        {
            tareButton.selectEntered.AddListener(OnTareButtonPressed);
        }
        
        UpdateScreen();
    }

    public void BeakerPlaced()
    {
        hasBeaker = true;
        currentWeight = emptyBeakerWeight;
        UpdateScreen();

        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.OnActionTriggered("PlaceBeakerOnScale");
        }
    }

    private void OnTareButtonPressed(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (hasBeaker)
        {
            currentWeight = 0.00f; 
            isTared = true;
            UpdateScreen();
            
            if (UniversalProcedureManager.Instance != null)
            {
                UniversalProcedureManager.Instance.OnActionTriggered("TareScale");
            }
        }
    }

    public bool AddKalk()
    {
        if (!hasBeaker) return false; // Mislukt: geen beker

        if (!isTared)
        {
            if (UniversalProcedureManager.Instance != null)
            {
                UniversalProcedureManager.Instance.ShowGlobalFailure("Fout! Je bent vergeten de weegschaal te tarreren. Je weegt nu het glas mee!");
            }
            return false; // Mislukt: niet getarreerd. Geef 'false' terug aan de triggerzone!
        }

        currentWeight = targetKalkWeight;
        UpdateScreen();
        
        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.OnActionTriggered("AddKalk");
        }
        
        return true; // Succes! Geef 'true' terug.
    }

    private void UpdateScreen()
    {
        if (displayLabel != null)
        {
            displayLabel.text = currentWeight.ToString("F2") + " g";
        }
    }

    // NIEUW: Wist het geheugen van de weegschaal bij een herstart
    public void ResetScale()
    {
        currentWeight = 0.00f;
        hasBeaker = false;
        isTared = false;
        UpdateScreen();
        Debug.Log("<color=white>[WeighingScale]</color> Logica van de weegschaal volledig gereset.");
    }
}