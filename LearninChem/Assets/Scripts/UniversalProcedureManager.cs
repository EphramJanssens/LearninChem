using Unity.VisualScripting;
using UnityEngine;

public class UniversalProcedureManager : MonoBehaviour
{
    public static UniversalProcedureManager Instance;
    [Header("Active Module Blueprint")]
    public ModuleData activeModule;
    private int currentStepIndex = 0;
    private string lastTriggeredAction = "";

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (activeModule != null)
        {
            StartModule(activeModule);
        }
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

        if (incomingActionID == lastTriggeredAction) 
        {
        return; 
        }

        string targetActionID = activeModule.stepActionIDs[currentStepIndex];

        if (incomingActionID == targetActionID)
        {
            Debug.Log($"<color=green>Correct!</color> Voltooid: {incomingActionID}");
            lastTriggeredAction = incomingActionID;
            currentStepIndex++;

            if (currentStepIndex >= activeModule.stepActionIDs.Length)
            {
                Debug.Log("<color=cyan>Module Voltooid!</color> Goed gewerkt!");
            }
            else
            {
                PrintCurrentStep();
            }
        }
        else
        {
            string  hint = activeModule.stepDescriptions[currentStepIndex];
            Debug.Log($"<color=red>Niet correct!</color> Hulp dialoog: Je moet eerst dit doen: {hint}");
        }
    }

    void PrintCurrentStep()
    {
        string currentInstructions = activeModule.stepDescriptions[currentStepIndex];
        Debug.Log($"<color=yellow>Nieuwe Taak:</color> {currentInstructions}");
    }
}
