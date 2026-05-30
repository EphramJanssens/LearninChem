using UnityEngine;

public class LiquidTriggerZone : MonoBehaviour
{
    [Header("Welk object verwachten we hier?")]
    public string requiredTag;

    [Header("Welke actie moeten we doorgeven?")]
    public string actionIDToSend;

    [Header("Anti-Spam Instellingen")]
    [Tooltip("Hoeveel seconden de trigger pauzeert na een aanraking.")]
    public float triggerCooldown = 2.0f;
    private float lastTriggerTime = -10f;

    private bool isPermanentlyLocked = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (isPermanentlyLocked) return;

        if (other.CompareTag(requiredTag))
        {
            if (Time.time - lastTriggerTime < triggerCooldown)
            {
                return; 
            }

            lastTriggerTime = Time.time;
            
            Debug.Log($"<color=cyan>[LiquidTrigger]</color> {requiredTag} gedetecteerd!");
            
            if (actionIDToSend == "AddIndicator" && TitrationController.Instance != null)
            {
                TitrationController.Instance.isBeakerPrepared = true;
                isPermanentlyLocked = true; 
                Debug.Log("<color=green>[LiquidTrigger]</color> Indicator toegevoegd! Trigger is nu permanent gelockt voor deze sessie.");
            }
            
            if (UniversalProcedureManager.Instance != null)
            {
                UniversalProcedureManager.Instance.OnActionTriggered(actionIDToSend);
            }
        }
    }

    public void ResetTrigger()
    {
        isPermanentlyLocked = false;
        lastTriggerTime = -10f;
    }
}