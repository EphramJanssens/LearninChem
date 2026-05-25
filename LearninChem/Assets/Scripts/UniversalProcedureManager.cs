using UnityEngine;
using UnityEngine.UIElements;

public class UniversalProcedureManager : MonoBehaviour
{
    public static UniversalProcedureManager Instance;
    
    [Header("Active Module Blueprint")]
    public ModuleData activeModule;
    
    [Header("UI Reference")]
    public UIDocument dashboardUI;
    private VisualElement inputSection;
    private Button toMainMenuBtn;

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
        if (dashboardUI != null && dashboardUI.rootVisualElement != null)
        {
            var root = dashboardUI.rootVisualElement;
            inputSection = root.Q<VisualElement>("InputSection");
            
            toMainMenuBtn = root.Q<Button>("ToMainMenuBtn");
            if (toMainMenuBtn != null)
            {
                toMainMenuBtn.clicked += ReturnToMainMenu;
                toMainMenuBtn.style.display = DisplayStyle.None;
            }

            ShowInputPanel(false);
        }
    }

    public void StartModule(ModuleData module)
    {
        activeModule = module;
        currentStepIndex = 0;
        isModuleActive = true; 
        
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.None;
        
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
                ShowInputPanel(false); 
                
                if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.Flex;

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
            if (PPEDashboardTester.Instance != null)
            {
                PPEDashboardTester.Instance.ShowFailure($"Let op! Je moet eerst dit doen: {hint}");
            }
        }
    }

    private void ReturnToMainMenu()
    {
        currentStepIndex = 0;
        isModuleActive = false;
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.None;

        ResettableProp[] allProps = FindObjectsByType<ResettableProp>(FindObjectsSortMode.None);
        foreach (ResettableProp prop in allProps)
        {
            prop.ResetToHome();
        }
        if (TitrationController.Instance != null) TitrationController.Instance.ResetLiquid();

        if (vrPlayer != null && spawnPoint != null)
        {
            CharacterController cc = vrPlayer.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            vrPlayer.position = spawnPoint.position;
            vrPlayer.rotation = spawnPoint.rotation;

            if (cc != null) cc.enabled = true;
            Debug.Log("<color=green>[Teleport]</color> Speler teruggezet naar spawnpoint.");
        }

        MainMenuController mainMenu = FindFirstObjectByType<MainMenuController>();
        if (mainMenu != null)
        {
            mainMenu.ShowMainMenu();
        }

        if (PPEDashboardTester.Instance != null)
        {
             PPEDashboardTester.Instance.ResetDashboardVisuals(); 
        }
    }

    public void ResetSimulation()
    {
        currentStepIndex = 0;
        isModuleActive = false; 
        ShowInputPanel(false);
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.None;

        ResettableProp[] allProps = FindObjectsByType<ResettableProp>(FindObjectsSortMode.None);
        foreach (ResettableProp prop in allProps)
        {
            prop.ResetToHome();
        }
        if (TitrationController.Instance != null) TitrationController.Instance.ResetLiquid();
    }

    void PrintCurrentStep()
    {
        if (activeModule == null || currentStepIndex >= activeModule.stepActionIDs.Length) return;

        string currentTargetID = activeModule.stepActionIDs[currentStepIndex];
        string currentInstructions = activeModule.stepDescriptions[currentStepIndex];
        string currentInfo = activeModule.stepInfo[currentStepIndex]; 

        if (PPEDashboardTester.Instance != null)
        {
            PPEDashboardTester.Instance.UpdateTaskPanel(currentInstructions, currentInfo);
        }

        if (currentTargetID == "SubmitValue")
        {
            ShowInputPanel(true);
        }
        else
        {
            ShowInputPanel(false);
        }
    }

    private void ShowInputPanel(bool show)
    {
        if (inputSection != null)
        {
            inputSection.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}