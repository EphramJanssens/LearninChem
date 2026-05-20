using UnityEngine;

public class UniversalProcedureManager : MonoBehaviour
{
    public static UniversalProcedureManager Instance;
    
    [Header("Active Module Blueprint")]
    public ModuleData activeModule;
    
    private int currentStepIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    public void StartModule(ModuleData module)
    {
        activeModule = module;
        currentStepIndex = 0;
        PrintCurrentStep();
    }

    public void OnActionTriggered(string incomingActionID)
    {
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
            // --- FOUTMELDING NAAR DE UI STUREN ---
            string hint = activeModule.stepDescriptions[currentStepIndex];
            Debug.Log($"<color=red>Niet correct!</color> Hint: {hint}");
            
            if (PPEDashboardTester.Instance != null)
            {
                PPEDashboardTester.Instance.ShowFailure($"Let op! Je moet eerst dit doen: {hint}");
            }
        }
    }

    void PrintCurrentStep()
    {
        if (currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string currentInstructions = activeModule.stepDescriptions[currentStepIndex];
        
        string currentInfo = activeModule.stepInfo[currentStepIndex]; 

        Debug.Log($"<color=yellow>Nieuwe Taak:</color> {currentInstructions}");

        // --- UPDATE HET TWEEDE SCHERM VAN DE UI ---
        if (PPEDashboardTester.Instance != null)
        {
            PPEDashboardTester.Instance.UpdateTaskPanel(currentInstructions, currentInfo);
        }
    }
}