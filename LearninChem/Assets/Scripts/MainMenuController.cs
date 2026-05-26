using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Koppeling")]
    public UIDocument mainMenuUI;

    [Header("Module Databestanden")]
    public ModuleData module1Data;
    public ModuleData module2Data;
    public ModuleData module3Data;

    private VisualElement mainMenuContainer;
    private Collider uiCollider;

    void OnEnable()
    {
        uiCollider = GetComponent<Collider>();

        if (mainMenuUI != null && mainMenuUI.rootVisualElement != null)
        {
            var root = mainMenuUI.rootVisualElement;
            mainMenuContainer = root.Q<VisualElement>("MainMenuContainer");

            Button btnMod1 = root.Q<Button>("BtnModule1");
            Button btnMod2 = root.Q<Button>("BtnModule2");
            Button btnMod3 = root.Q<Button>("BtnModule3");
            
            Button btnExit = root.Q<Button>("BtnExitApp");

            if (btnMod1 != null) btnMod1.clicked += () => StartSelectedModule(module1Data);
            if (btnMod2 != null) btnMod2.clicked += () => StartSelectedModule(module2Data);
            if (btnMod3 != null) btnMod3.clicked += () => StartSelectedModule(module3Data);
            
            if (btnExit != null) btnExit.clicked += QuitApplication;
        }
    }

    private void StartSelectedModule(ModuleData selectedData)
    {
        if (selectedData == null)
        {
            Debug.LogWarning("[MainMenu] Geen ModuleData gekoppeld in de Inspector!");
            return;
        }

        if (mainMenuContainer != null) mainMenuContainer.style.display = DisplayStyle.None;
        
        if (uiCollider != null) uiCollider.enabled = false;

        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.StartModule(selectedData);
            Debug.Log($"<color=cyan>[MainMenu]</color> {selectedData.name} opgestart!");
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuContainer != null) mainMenuContainer.style.display = DisplayStyle.Flex;
        
        if (uiCollider != null) uiCollider.enabled = true;
    }

    private void QuitApplication()
    {
        Debug.Log("<color=red>[MainMenu]</color> Applicatie wordt afgesloten...");
        
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}