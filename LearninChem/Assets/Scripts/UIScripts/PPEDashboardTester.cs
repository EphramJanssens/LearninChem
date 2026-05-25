using UnityEngine;
using UnityEngine.UIElements;

public class PPEDashboardTester : MonoBehaviour
{
    public static PPEDashboardTester Instance;

    private UIDocument uiDocument;
    
    private VisualElement startMenuContainer;
    private VisualElement taskPanelContainer;

    private Label instructionText;
    private Label infoText;
    private Button startButton;
    private Button restartButton;

    private VisualElement helpPanel;
    private Label helpMessage;

    void Awake()
    {
        Instance = this;
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        startMenuContainer = root.Q<VisualElement>("StartMenuContainer");
        taskPanelContainer = root.Q<VisualElement>("TaskPanelContainer");

        instructionText = root.Q<Label>("InstructionText");
        infoText = root.Q<Label>("InfoText");
        startButton = root.Q<Button>("StartTrainingBtn");

        helpPanel = root.Q<VisualElement>("HelpPanel");
        helpMessage = root.Q<Label>("HelpMessage");

        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;

        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }

        restartButton = root.Q<Button>("RestartBtn");

        if (restartButton != null)
        {
            restartButton.clicked += OnRestartButtonClicked;
        }
    }

    void OnStartButtonClicked()
    {
        if (startMenuContainer != null) startMenuContainer.style.display = DisplayStyle.None;
        if (taskPanelContainer != null) taskPanelContainer.style.display = DisplayStyle.Flex;

        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.StartModule(UniversalProcedureManager.Instance.activeModule);
        }
    }

    void OnRestartButtonClicked()
    {
        if (startMenuContainer != null) startMenuContainer.style.display = DisplayStyle.Flex;
        if (taskPanelContainer != null) taskPanelContainer.style.display = DisplayStyle.None;
        if (restartButton != null) restartButton.style.display = DisplayStyle.None;
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;

       if (UniversalProcedureManager.Instance != null)
       {
            UniversalProcedureManager.Instance.ResetSimulation();
       }
    }

    public void UpdateTaskPanel(string instruction, string dynamicInfo)
    {
        if (instructionText != null) instructionText.text = instruction;
        if (infoText != null) infoText.text = dynamicInfo;
        
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
    }
    
    public void ShowFailure(string message)
    {
        if (helpMessage != null) helpMessage.text = message;
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.Flex;
    }

    public void ShowModuleComplete()
    {
        if (instructionText != null) instructionText.text = "Module Voltooid!";
        if (infoText != null) infoText.text = "Goed gewerkt! Je hebt alle PBM's correct aangetrokken.";
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
    
        if (restartButton != null) restartButton.style.display = DisplayStyle.Flex;
    }

    public void ResetDashboardVisuals()
    {
        var uiDocument = GetComponent<UnityEngine.UIElements.UIDocument>();
        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            var root = uiDocument.rootVisualElement;

            var startMenu = root.Q<UnityEngine.UIElements.VisualElement>("StartMenuContainer");
            var taskPanel = root.Q<UnityEngine.UIElements.VisualElement>("TaskPanelContainer");
            var helpPanel = root.Q<UnityEngine.UIElements.VisualElement>("HelpPanel");
            var restartBtn = root.Q<UnityEngine.UIElements.Button>("RestartBtn");

            if (startMenu != null) startMenu.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
            if (taskPanel != null) taskPanel.style.display = UnityEngine.UIElements.DisplayStyle.None;
            if (helpPanel != null) helpPanel.style.display = UnityEngine.UIElements.DisplayStyle.None;
            if (restartBtn != null) restartBtn.style.display = UnityEngine.UIElements.DisplayStyle.None;
            
            Debug.Log("<color=white>[Dashboard]</color> Visuals zijn gereset voor het hoofdmenu.");
        }
    }
}