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

    public void AddKalk()
    {
        if (!hasBeaker) return;

        if (!isTared)
        {
            if (PPEDashboardTester.Instance != null)
            {
                PPEDashboardTester.Instance.ShowFailure("Fout! Je bent vergeten de weegschaal te tarreren. Je weegt nu het glas mee!");
            }
            return; 
        }

        currentWeight = targetKalkWeight;
        UpdateScreen();
        
        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.OnActionTriggered("AddKalk");
        }
    }

    private void UpdateScreen()
    {
        if (displayLabel != null)
        {
            displayLabel.text = currentWeight.ToString("F2") + " g";
        }
    }
}