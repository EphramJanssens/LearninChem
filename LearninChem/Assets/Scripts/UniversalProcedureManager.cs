using UnityEngine;
using UnityEngine.UIElements; // Vergeet deze niet, nodig voor UI Toolkit!

public class UniversalProcedureManager : MonoBehaviour
{
    public static UniversalProcedureManager Instance;
    
    [Header("Active Module Blueprint")]
    public ModuleData activeModule;
    
    [Header("UI Reference")]
    public UIDocument dashboardUI; // Koppel hier je dashboard aan in de Inspector
    private VisualElement inputSection;
    
    private int currentStepIndex = 0;
    private bool isModuleActive = false; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Zoek de InputSection op in de UI Toolkit zodra de game start
        if (dashboardUI != null && dashboardUI.rootVisualElement != null)
        {
            inputSection = dashboardUI.rootVisualElement.Q<VisualElement>("InputSection");
            ShowInputPanel(false); // Zorg dat het standaard verborgen is
        }
    }

    public void StartModule(ModuleData module)
    {
        activeModule = module;
        currentStepIndex = 0;
        isModuleActive = true; 
        PrintCurrentStep();
    }

    public void OnActionTriggered(string incomingActionID)
    {
        if (!isModuleActive) return; 
        if (activeModule == null) return;
        if (currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string targetActionID = activeModule.stepActionIDs[currentStepIndex];

        if (incomingActionID == targetActionID)
        {
            Debug.Log($"<color=green>Correct!</color> Voltooid: {incomingActionID}");
            currentStepIndex++;

            if (currentStepIndex >= activeModule.stepActionIDs.Length)
            {
                Debug.Log("<color=cyan>Module Voltooid!</color> Goed gewerkt!");
                
                // Zorg dat het invoerveld verdwijnt op het eindscherm!
                ShowInputPanel(false); 
                
                if (PPEDashboardTester.Instance != null)
                {
                    PPEDashboardTester.Instance.ShowModuleComplete();
                }
            }
            else
            {
                PrintCurrentStep();
            }
        }
        else
        {
            string hint = activeModule.stepDescriptions[currentStepIndex];
            Debug.Log($"<color=red>Niet correct!</color> Hint: {hint}");
            
            if (PPEDashboardTester.Instance != null)
            {
                PPEDashboardTester.Instance.ShowFailure($"Let op! Je moet eerst dit doen: {hint}");
            }
        }
    }

    public void ResetSimulation()
    {
        currentStepIndex = 0;
        isModuleActive = false; 
        
        ShowInputPanel(false); // Verberg de invoer bij een reset

        ResettableProp[] allProps = FindObjectsByType<ResettableProp>(FindObjectsSortMode.None);
        foreach (ResettableProp prop in allProps)
        {
            prop.ResetToHome();
        }

        if (TitrationController.Instance != null) TitrationController.Instance.ResetLiquid();

        Debug.Log("<color=magenta>Simulatie is volledig gereset!</color>");
    }

    void PrintCurrentStep()
    {
        if (activeModule == null || currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string currentTargetID = activeModule.stepActionIDs[currentStepIndex];
        string currentInstructions = activeModule.stepDescriptions[currentStepIndex];
        string currentInfo = activeModule.stepInfo[currentStepIndex]; 

        Debug.Log($"<color=yellow>Nieuwe Taak:</color> {currentInstructions}");

        if (PPEDashboardTester.Instance != null)
        {
            PPEDashboardTester.Instance.UpdateTaskPanel(currentInstructions, currentInfo);
        }

        // Check of de huidige stap de validatie-stap is, en toon/verberg het nummerblok
        if (currentTargetID == "SubmitValue")
        {
            ShowInputPanel(true);
        }
        else
        {
            ShowInputPanel(false);
        }
    }

    // Handige helper functie om de zichtbaarheid veilig te schakelen
    private void ShowInputPanel(bool show)
    {
        if (inputSection != null)
        {
            inputSection.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}