using UnityEngine;

public class LiquidTriggerZone : MonoBehaviour
{
    [Header("Welk object verwachten we hier?")]
    public string requiredTag; // Bv. "IndicatorBottle" of "SampleBottle"

    [Header("Welke actie moeten we doorgeven?")]
    public string actionIDToSend; // Bv. "AddIndicator"

    private void OnTriggerEnter(Collider other)
    {
        // Controleer of het object dat binnenkomt de juiste Tag heeft
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