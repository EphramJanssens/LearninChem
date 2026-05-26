using UnityEngine;

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