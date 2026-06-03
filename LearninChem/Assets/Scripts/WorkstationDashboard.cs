using UnityEngine;
using UnityEngine.UIElements;

/* 
 * Functie: Bestuurt de lokale UI-schermen bij de werktafels (instructies, info en foutmeldingen) via Unity's UI Toolkit.
 * Invloed: Krijgt zijn teksten rechtstreeks gevoerd door de UniversalProcedureManager en stuurt commando's (zoals 'Herstarten' of 'Terug naar hoofdmenu') terug naar deze manager bij het indrukken van een knop.
 */

public class WorkstationDashboard : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label headerLabel;
    private Label instructionLabel;
    private Label infoLabel;
    private VisualElement taskPanelContainer;
    private VisualElement startMenuContainer;
    private VisualElement helpPanel;
    private Label helpMessage;

    private Button restartBtn;
    private Button toMainMenuBtn;
    private Button cancelBtn;

    private bool uiIsInitialized = false;

    void OnEnable()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        if (uiIsInitialized) return;

        uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            var root = uiDocument.rootVisualElement;
            headerLabel = root.Q<Label>("Header");
            instructionLabel = root.Q<Label>("InstructionText");
            infoLabel = root.Q<Label>("InfoText");
            taskPanelContainer = root.Q<VisualElement>("TaskPanelContainer");
            startMenuContainer = root.Q<VisualElement>("StartMenuContainer");
            helpPanel = root.Q<VisualElement>("HelpPanel");
            helpMessage = root.Q<Label>("HelpMessage");

            restartBtn = root.Q<Button>("RestartBtn");
            toMainMenuBtn = root.Q<Button>("ToMainMenuBtn");
            
            cancelBtn = root.Q<Button>("BtnCancelModule");

            if (restartBtn != null) restartBtn.clicked += OnRestartClicked;
            if (toMainMenuBtn != null) toMainMenuBtn.clicked += OnToMainMenuClicked;
            
            if (cancelBtn != null) cancelBtn.clicked += OnToMainMenuClicked; 

            uiIsInitialized = true;
        }
    }

/*
* Maakt het taak paneel zichtbaar en vult dit met de nieuwe teksten die het doorkrijgt van de manager.
*/
    public void UpdateDashboard(string title, string instruction, string info)
    {
        InitializeUI();

        Collider uiCollider = GetComponent<Collider>();
        if (uiCollider != null) uiCollider.enabled = true;

        if (startMenuContainer != null) startMenuContainer.style.display = DisplayStyle.None;
        if (taskPanelContainer != null) taskPanelContainer.style.display = DisplayStyle.Flex;
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;

        if (restartBtn != null) restartBtn.style.display = DisplayStyle.None;
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.None;
        
        if (cancelBtn != null) cancelBtn.style.display = DisplayStyle.Flex;

        if (headerLabel != null) headerLabel.text = title;
        if (instructionLabel != null) instructionLabel.text = instruction;
        if (infoLabel != null) infoLabel.text = info;
    }

/*
* Toont het rode waarschuwingspaneel (HelpPanel) met de specifieke foutmelding die de speler heeft gemaakt.
*/
    public void ShowFailure(string message)
    {
        InitializeUI();
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.Flex;
        if (helpMessage != null) helpMessage.text = message;
    }

/*
* Verandert de UI naar een succes scherm en toont de knoppen om opnieuw te beginnen of terug te keren.
*/
    public void SetModuleComplete()
    {
        InitializeUI();
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
        
        if (instructionLabel != null) instructionLabel.text = "Module Voltooid! Goed gewerkt!";
        if (infoLabel != null) infoLabel.text = "Gebruik de knoppen hieronder om een actie te kiezen.";

        if (restartBtn != null) restartBtn.style.display = DisplayStyle.Flex;
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.Flex;
        
        if (cancelBtn != null) cancelBtn.style.display = DisplayStyle.None;
    }

/*
* Bepaald welke panelen zichtbaar moeten zijn (bijv. het startscherm) en zetten de onzichtbare collider aan of uit zodat de speler er wel of niet op kan klikken.
*/
    public void ResetDashboard()
    {
        InitializeUI();
        if (startMenuContainer != null) startMenuContainer.style.display = DisplayStyle.Flex;
        if (taskPanelContainer != null) taskPanelContainer.style.display = DisplayStyle.None;
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
        
        if (restartBtn != null) restartBtn.style.display = DisplayStyle.None;
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.None;
        if (cancelBtn != null) cancelBtn.style.display = DisplayStyle.None;

        Collider uiCollider = GetComponent<Collider>();
        if (uiCollider != null) uiCollider.enabled = true;
    }

    public void DeactivateDashboard()
    {
        InitializeUI();
        if (startMenuContainer != null) startMenuContainer.style.display = DisplayStyle.None;
        if (taskPanelContainer != null) taskPanelContainer.style.display = DisplayStyle.None;
        if (helpPanel != null) helpPanel.style.display = DisplayStyle.None;
        
        if (restartBtn != null) restartBtn.style.display = DisplayStyle.None;
        if (toMainMenuBtn != null) toMainMenuBtn.style.display = DisplayStyle.None;
        if (cancelBtn != null) cancelBtn.style.display = DisplayStyle.None;

        Collider uiCollider = GetComponent<Collider>();
        if (uiCollider != null) uiCollider.enabled = false;
    }

/*
* Event listeners die doorgeven aan de UniversalProcedureManager welke knop de speler heeft ingedrukt.
*/
    private void OnRestartClicked()
    {
        if (UniversalProcedureManager.Instance != null && UniversalProcedureManager.Instance.activeModule != null)
        {
            Debug.Log("<color=orange>[Dashboard]</color> Herstarten van de huidige module getriggerd...");
            UniversalProcedureManager.Instance.StartModule(UniversalProcedureManager.Instance.activeModule);
        }
    }

    private void OnToMainMenuClicked()
    {
        if (UniversalProcedureManager.Instance != null)
        {
            Debug.Log("<color=orange>[Dashboard]</color> Terugkeren/Afbreken naar hoofdmenu getriggerd...");
            UniversalProcedureManager.Instance.ReturnToMainMenu();
        }
    }
}