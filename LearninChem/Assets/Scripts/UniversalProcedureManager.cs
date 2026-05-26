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

        // 1. Reset alle dashboards én alle fysieke objecten op de tafels!
        ResetAllDashboards();
        ResetPhysicalProps(); // <--- OPGELOST: Fysieke objecten worden nu ook bij herstart gereset!
        
        // 2. Bepaal welk dashboard bij welke module hoort
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

    // NIEUWE CENTRALE FUNCTIE: Reset alle fysieke laboratorium objecten
    private void ResetPhysicalProps()
    {
        // 1. Reset alle objecten met het 'ResettableProp' script (positie/rotatie)
        ResettableProp[] allProps = FindObjectsByType<ResettableProp>(FindObjectsSortMode.None);
        foreach (ResettableProp prop in allProps)
        {
            prop.ResetToHome();
        }

        // 2. Reset de titratievloeistof (Module 2)
        if (TitrationController.Instance != null)
        {
            TitrationController.Instance.ResetLiquid();
        }

        // 3. Reset de roer-detectors (Module 3) zodat de timer weer op 0 springt!
        StirDetector[] allStirDetectors = FindObjectsByType<StirDetector>(FindObjectsSortMode.None);
        foreach (StirDetector stir in allStirDetectors)
        {
            stir.ResetStirring();
        }

        // --- NIEUW: 4. Reset de KalkTriggerZone ---
        KalkTriggerZone[] allKalkTriggers = FindObjectsByType<KalkTriggerZone>(FindObjectsSortMode.None);
        foreach (KalkTriggerZone kalk in allKalkTriggers)
        {
            kalk.ResetTrigger();
        }

        // --- NIEUW: 5. Reset de Geleidbaarheidsmeter (Module 3) ---
        if (ConductivityMeter.Instance != null)
        {
            ConductivityMeter.Instance.ResetMeter();
        }

        // --- NIEUW: 6. Reset het Numpad invoerveld (Module 3) ---
        DashboardInputValidator[] allValidators = FindObjectsByType<DashboardInputValidator>(FindObjectsSortMode.None);
        foreach (DashboardInputValidator validator in allValidators)
        {
            validator.ResetValidator();
        }

        // --- NIEUW: 7. Reset de Weegschaal Logica (Module 3) ---
        WeighingScale[] allScales = FindObjectsByType<WeighingScale>(FindObjectsSortMode.None);
        foreach (WeighingScale scale in allScales)
        {
            scale.ResetScale();
        }

        Debug.Log("<color=orange>[Manager]</color> Alle fysieke laboratorium objecten en detectors zijn gereset.");
    }

    public void ReturnToMainMenu()
    {
        currentStepIndex = 0;
        isModuleActive = false;

        ResetAllDashboards();
        ResetPhysicalProps(); // Schone herstart voor het hoofdmenu

        // 1. Teleportatie van de VR-speler
        if (vrPlayer != null && spawnPoint != null)
        {
            CharacterController cc = vrPlayer.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            vrPlayer.position = spawnPoint.position;
            vrPlayer.rotation = spawnPoint.rotation;

            if (cc != null) cc.enabled = true;
        }

        // 2. Toon het hoofdmenu weer
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