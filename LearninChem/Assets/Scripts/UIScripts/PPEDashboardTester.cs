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
    }
}