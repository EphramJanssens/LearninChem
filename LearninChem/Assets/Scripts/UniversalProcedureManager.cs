using UnityEngine;
using UnityEngine.UIElements;

public class UniversalProcedureManager : MonoBehaviour
{
    public static UniversalProcedureManager Instance;
    
    [Header("Active Module Blueprint")]
    public ModuleData activeModule;

    [Header("Gekoppelde Werkplekken")]
    public WorkstationDashboard dashboardModule1;
    public WorkstationDashboard dashboardModule2;
    public WorkstationDashboard dashboardModule3;

    private WorkstationDashboard currentActiveDashboard;

    [Header("VR Player & Spawnpoint")]
    public Transform vrPlayer;       
    public Transform spawnPoint;     
    
    private int currentStepIndex = 0;
    private bool isModuleActive = false; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetAllDashboards();
    }

    public void StartModule(ModuleData module)
    {
        activeModule = module;
        currentStepIndex = 0;
        isModuleActive = true; 

        ResetAllDashboards();
        ResetPhysicalProps();
        
        if (module.name.Contains("Module1") || module.moduleTitle.Contains("PBM")) currentActiveDashboard = dashboardModule1;
        else if (module.name.Contains("Module2") || module.moduleTitle.Contains("Titratie")) currentActiveDashboard = dashboardModule2;
        else currentActiveDashboard = dashboardModule3;

        PrintCurrentStep();
    }

    public void OnActionTriggered(string incomingActionID)
    {
        if (!isModuleActive || activeModule == null) return;
        if (currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string targetActionID = activeModule.stepActionIDs[currentStepIndex];

        if (incomingActionID == targetActionID)
        {
            Debug.Log($"<color=green>Correct!</color> Voltooid: {incomingActionID}");
            currentStepIndex++;

            if (currentStepIndex >= activeModule.stepActionIDs.Length)
            {
                isModuleActive = false;
                if (currentActiveDashboard != null) currentActiveDashboard.SetModuleComplete();
            }
            else
            {
                PrintCurrentStep();
            }
        }
        else
        {
            string hint = activeModule.stepDescriptions[currentStepIndex];
            ShowGlobalFailure($"Let op! Je moet eerst dit doen: {hint}");
        }
    }

    void PrintCurrentStep()
    {
        if (activeModule == null || currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string currentInstructions = activeModule.stepDescriptions[currentStepIndex];
        string currentInfo = activeModule.stepInfo[currentStepIndex]; 

        if (currentActiveDashboard != null)
        {
            currentActiveDashboard.UpdateDashboard(activeModule.moduleTitle, currentInstructions, currentInfo);
        }
    }

    private void ResetAllDashboards()
    {
        if (dashboardModule1 != null) dashboardModule1.DeactivateDashboard();
        if (dashboardModule2 != null) dashboardModule2.DeactivateDashboard();
        if (dashboardModule3 != null) dashboardModule3.DeactivateDashboard();
    }

    private void ResetPhysicalProps()
    {
        ResettableProp[] allProps = FindObjectsByType<ResettableProp>(FindObjectsSortMode.None);
        foreach (ResettableProp prop in allProps)
        {
            prop.ResetToHome();
        }

        if (TitrationController.Instance != null)
        {
            TitrationController.Instance.ResetLiquid();
        }

        StirDetector[] allStirDetectors = FindObjectsByType<StirDetector>(FindObjectsSortMode.None);
        foreach (StirDetector stir in allStirDetectors)
        {
            stir.ResetStirring();
        }

        KalkTriggerZone[] allKalkTriggers = FindObjectsByType<KalkTriggerZone>(FindObjectsSortMode.None);
        foreach (KalkTriggerZone kalk in allKalkTriggers)
        {
            kalk.ResetTrigger();
        }

        if (ConductivityMeter.Instance != null)
        {
            ConductivityMeter.Instance.ResetMeter();
        }

        DashboardInputValidator[] allValidators = FindObjectsByType<DashboardInputValidator>(FindObjectsSortMode.None);
        foreach (DashboardInputValidator validator in allValidators)
        {
            validator.ResetValidator();
        }

        WeighingScale[] allScales = FindObjectsByType<WeighingScale>(FindObjectsSortMode.None);
        foreach (WeighingScale scale in allScales)
        {
            scale.ResetScale();
        }

        // --- NIEUW: Reset de Handschoenen (Module 1) ---
        GloveDispenser[] allDispensers = FindObjectsByType<GloveDispenser>(FindObjectsSortMode.None);
        foreach (GloveDispenser dispenser in allDispensers)
        {
            dispenser.ResetGloves();
        }

        Debug.Log("<color=orange>[Manager]</color> Alle fysieke laboratorium objecten en detectors zijn gereset.");
    }

    public void ReturnToMainMenu()
    {
        currentStepIndex = 0;
        isModuleActive = false;

        ResetAllDashboards();
        ResetPhysicalProps();

        if (vrPlayer != null && spawnPoint != null)
        {
            CharacterController cc = vrPlayer.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            vrPlayer.position = spawnPoint.position;
            vrPlayer.rotation = spawnPoint.rotation;

            if (cc != null) cc.enabled = true;
        }

        MainMenuController mainMenu = FindFirstObjectByType<MainMenuController>();
        if (mainMenu != null)
        {
            mainMenu.ShowMainMenu();
        }
    }

    public void ShowGlobalFailure(string message)
    {
        if (currentActiveDashboard != null)
        {
            currentActiveDashboard.ShowFailure(message);
        }
        else
        {
            Debug.LogWarning($"<color=red>Fout (Geen dashboard actief):</color> {message}");
        }
    }
}