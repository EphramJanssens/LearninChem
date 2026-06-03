using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/*
 * Functie: Simuleert een weegschaal met een tarre functie en weergave op een lokaal UI scherm. Controleert of er getarreerd is voordat kalk wordt toegevoegd.
 * Invloed: Triggert visuele kleur updates in BeakerVisuals en geeft acties (zoals 'TareScale' of 'AddKalk') of foutmeldingen door aan de UniversalProcedureManager.
 */

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
        if (!hasBeaker) return false;

        if (!isTared)
        {
            if (UniversalProcedureManager.Instance != null)
            {
                UniversalProcedureManager.Instance.ShowGlobalFailure("Fout! Je bent vergeten de weegschaal te tarreren. Je weegt nu het glas mee!");
            }
            return false;
        }

        currentWeight = targetKalkWeight;
        UpdateScreen();

        if (BeakerVisuals.Instance != null)
        {
            BeakerVisuals.Instance.ToonKalk();
        }
        
        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.OnActionTriggered("AddKalk");
        }
        
        return true;
    }

    private void UpdateScreen()
    {
        if (displayLabel != null)
        {
            displayLabel.text = currentWeight.ToString("F2") + " g";
        }
    }

    public void ResetScale()
    {
        currentWeight = 0.00f;
        hasBeaker = false;
        isTared = false;
        UpdateScreen();
        Debug.Log("<color=white>[WeighingScale]</color> Logica van de weegschaal volledig gereset.");
    }
}