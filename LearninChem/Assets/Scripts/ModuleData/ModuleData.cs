using UnityEngine;

/* 
 * Functie: Een ScriptableObject (data container) die de blauwdruk van een trainingsmodule opslaat, zoals de naam, de juiste volgorde van acties (stepActionIDs) en de instructieteksten.
 * Invloed: Bevat geen actieve code, maar dicteert volledig hoe de MainMenuController en UniversalProcedureManager de module opbouwen en valideren.
 */

[CreateAssetMenu(fileName = "NewModuleData", menuName = "Scriptable Objects/ModuleData")]
public class ModuleData : ScriptableObject
{
    [Header("Module Instellingen")]
    public string moduleTitle;
    
    [Header("Stappen & Logica")]
    public string[] stepActionIDs;
    
    [TextArea(2, 5)]
    public string[] stepDescriptions;
    
    [TextArea(3, 10)]
    public string[] stepInfo;
}