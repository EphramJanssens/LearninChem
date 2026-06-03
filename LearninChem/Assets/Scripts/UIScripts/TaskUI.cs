using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskUI : MonoBehaviour
{
    private UIDocument uiDocument;

    private Label instructionLabel;
    private Label infoLabel;
    private VisualElement helpPanel;
    private Label helpMessageLabel;
    private VisualElement completionPanel;

/* 
 * Functie: Een generieke UI-controller om simpelweg instructies, waarschuwingen en succes-statussen op een UIDocument te tonen.
 * Invloed: Ontvangt commando's van buitenaf om teksten te updaten, maar beïnvloedt zelf geen andere logica of scripts.
 */

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        instructionLabel = root.Q<Label>("instruction-text");
        infoLabel = root.Q<Label>("info-text");

        helpPanel = root.Q<VisualElement>("help-panel");
        helpMessageLabel = root.Q<Label>("help-message");

        completionPanel = root.Q<VisualElement>("completion-panel");

        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
        if (completionPanel != null) completionPanel.style.display = DisplayStyle.None;
    }

    public void UpdateStep(string instruction, string info)
    {
        if (instructionLabel != null) instructionLabel.text = instruction;
        if (infoLabel != null) infoLabel.text = info;

        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
    }

    public void ShowFailure(string message)
    {
        if (helpMessageLabel != null) helpMessageLabel.text = message;
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.Flex;
    }

    public void ShowComplete()
    {
        if (completionPanel != null) completionPanel.style.display = DisplayStyle.Flex;
    }
}
