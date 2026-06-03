using UnityEngine;
using UnityEngine.UIElements;

/* 
 * Functie: Beheert de UI van het startscherm en verwerkt de knopklikken voor de module selectie en het afsluiten van de app.
 * Invloed: Leest de ModuleData (ScriptableObjects) in en activeert de gekozen module door UniversalProcedureManager.StartModule() aan te roepen.
 */

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

/*
* Koppelt het UI document en voegt klik events toe aan de module knoppen en de afsluit-knop.
*/
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

/*
* Verbergt het hoofdmenu,
* zet de menu collider uit (zodat spelers er niet per ongeluk in de achtergrond op klikken)
* en geeft het gekozen startbestand door aan de UniversalProcedureManager.
*/
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

/*
* Verbergt het hoofdmenu,
*/
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