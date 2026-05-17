using UnityEngine;
using UnityEngine.UIElements;

public class PPEDashboardTester : MonoBehaviour
{
    public static PPEDashboardTester Instance;

    private UIDocument uiDocument;
    
    // References to your 3 UXML containers
    private VisualElement panelGoggles;
    private VisualElement panelGloves;
    private VisualElement panelCoat;
    private Button startButton;

    // Define colors using clean hex design principles
    private readonly Color normalColor = new Color(0.10f, 0.11f, 0.14f); // rgb(26, 29, 36)
    private readonly Color successColor = new Color(0.02f, 0.38f, 0.24f); // Clean Dark Green

    void Awake()
    {
    Instance = this;
    uiDocument = GetComponent<UIDocument>();
    var root = uiDocument.rootVisualElement;

    // FIX: Match the names exactly as written in your UXML text!
    panelGoggles = root.Q<VisualElement>("PPEInfoPanel1"); // Linked to your first panel
    panelGloves = root.Q<VisualElement>("PPEInfoPanel2");  // Linked to your second panel
    panelCoat = root.Q<VisualElement>("PPEInfoPanel3");    // Linked to your third panel
    startButton = root.Q<Button>("StartTrainingBtn");

    if (startButton != null)
    {
        startButton.clicked += () => Debug.Log("<color=cyan>Startknop ingedrukt!</color> Start de procedure.");
    }
    }

    // Call this function when a task succeeds
    public void SetStepComplete(string actionID)
    {
        switch (actionID)
        {
            case "Goggles":
                ApplySuccessStyle(panelGoggles);
                break;
            case "LabCoat":
                ApplySuccessStyle(panelCoat);
                break;
            case "Gloves":
                ApplySuccessStyle(panelGloves);
                break;
        }
    }

    private void ApplySuccessStyle(VisualElement element)
    {
        if (element == null) return;

        // Visual feedback: Shift background to green and apply a solid thick border
        element.style.backgroundColor = successColor;
        element.style.borderTopColor = Color.green;
        element.style.borderBottomColor = Color.green;
        element.style.borderLeftColor = Color.green;
        element.style.borderRightColor = Color.green;
        
        element.style.borderTopWidth = 3;
        element.style.borderBottomWidth = 3;
        element.style.borderLeftWidth = 3;
        element.style.borderRightWidth = 3;
    }
}
