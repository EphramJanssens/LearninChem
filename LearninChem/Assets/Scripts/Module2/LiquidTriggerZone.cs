using UnityEngine;

public class LiquidTriggerZone : MonoBehaviour
{
    [Header("Welk object verwachten we hier?")]
    public string requiredTag;

    [Header("Welke actie moeten we doorgeven?")]
    public string actionIDToSend;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag(requiredTag))
        {
            hasTriggered = true;
            Debug.Log($"<color=cyan>[LiquidTrigger]</color> {requiredTag} gedetecteerd en gelockt!");
            
            if (actionIDToSend == "AddIndicator" && TitrationController.Instance != null)
            {
                TitrationController.Instance.isBeakerPrepared = true;
                Debug.Log("<color=green>[LiquidTrigger]</color> Indicator toegevoegd! De beker is nu klaar voor titratie.");
            }
            
            if (UniversalProcedureManager.Instance != null)
            {
                UniversalProcedureManager.Instance.OnActionTriggered(actionIDToSend);
            }
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}