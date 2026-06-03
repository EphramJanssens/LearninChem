using UnityEngine;
using UnityEngine.UIElements;

/* 
 * Functie: Het centrale hart (Singleton) van de applicatie dat de logica, voortgang en veiligheidscontroles van de actieve module beheert.
 * Invloed: Ontvangt acties van alle objecten in de wereld, vergelijkt deze met de ModuleData, stuurt de WorkstationDashboards aan met nieuwe tekst, en activeert de reset-functies van alle ResettableProps en controllers in de scene.
 */

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

    [Header("Audio Feedback")]
    public AudioSource sfxPlayer;
    public AudioClip successClip;
    public AudioClip failureClip;
    
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

/*
* Laadt de gekozen blauwdruk in.
* reset de fysieke wereld.
* teleporteert de speler en activeert het juiste dashboard voor die module.
*/
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

/*
* Vergelijkt een inkomende actie (bijv. "OpenValve") met het verwachte stappenplan.
* Bij succes gaat de speler naar de volgende stap; bij een fout triggert dit een waarschuwing.
*/
    public void OnActionTriggered(string incomingActionID)
    {
        if (!isModuleActive || activeModule == null) return;
        if (currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string targetActionID = activeModule.stepActionIDs[currentStepIndex];

        if (incomingActionID == targetActionID)
        {
            Debug.Log($"<color=green>Correct!</color> Voltooid: {incomingActionID}");
            currentStepIndex++;

            if (sfxPlayer != null && successClip != null)
            {
            sfxPlayer.PlayOneShot(successClip);
            }

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

/*
* Haalt de actuele instructieteksten uit de ModuleData en stuurt deze naar het actieve dashboard.
*/
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

/*
* Deactiveert alle werktafel schermen.
*/
    private void ResetAllDashboards()
    {
        if (dashboardModule1 != null) dashboardModule1.DeactivateDashboard();
        if (dashboardModule2 != null) dashboardModule2.DeactivateDashboard();
        if (dashboardModule3 != null) dashboardModule3.DeactivateDashboard();
    }

/*
* Zoekt alle interactieve objecten (weegschaal, bekers, meters) in de ruimte en roept hun individuele reset functies aan om de ruimte op te ruimen.
*/
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

        GloveDispenser[] allDispensers = FindObjectsByType<GloveDispenser>(FindObjectsSortMode.None);
        foreach (GloveDispenser dispenser in allDispensers)
        {
            dispenser.ResetGloves();
        }

        LiquidTriggerZone[] allLiquidTriggers = FindObjectsByType<LiquidTriggerZone>(FindObjectsSortMode.None);
        foreach (LiquidTriggerZone liquidTrigger in allLiquidTriggers)
        {
            liquidTrigger.ResetTrigger();
        }

        if (BeakerVisuals.Instance != null)
        {
            BeakerVisuals.Instance.ResetBeker();
        }

        Debug.Log("<color=orange>[Manager]</color> Alle fysieke laboratorium objecten en detectors zijn gereset.");
    }

/*
* Breekt de huidige sessie af, reset de wereld, teleporteert de speler terug naar het startpunt en toont het hoofdmenu.
*/
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

/*
* Stuurt een rode foutmelding inclusief geluidseffect naar het actieve dashboard.
*/
    public void ShowGlobalFailure(string message)
    {
        if (currentActiveDashboard != null)
        {
            currentActiveDashboard.ShowFailure(message);

            if (sfxPlayer != null && failureClip != null)
            {
            sfxPlayer.PlayOneShot(failureClip);
            }
        }
        else
        {
            Debug.LogWarning($"<color=red>Fout (Geen dashboard actief):</color> {message}");
        }
    }
}