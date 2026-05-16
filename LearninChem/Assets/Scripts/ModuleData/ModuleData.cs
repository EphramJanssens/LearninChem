using UnityEngine;

[CreateAssetMenu(fileName = "ModuleData", menuName = "Scriptable Objects/ModuleData")]
public class ModuleData : ScriptableObject
{
    public string moduleName;
    public string[] stepActionIDs;
    public string[] stepDescriptions;
}
