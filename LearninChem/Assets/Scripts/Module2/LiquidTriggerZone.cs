using UnityEngine;

public class LiquidTriggerZone : MonoBehaviour
{
    [Header("Welk object verwachten we hier?")]
    public string requiredTag;

    [Header("Welke actie moeten we doorgeven?")]
    public string actionIDToSend;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            Debug.Log($"<color=cyan>[LiquidTrigger]</color> {requiredTag} gedetecteerd!");
            
            // Stuur het door naar onze trouwe manager!
            if (UniversalProcedureManager.Instance != null)
            {
                UniversalProcedureManager.Instance.OnActionTriggered(actionIDToSend);
            }
        }
    }
}