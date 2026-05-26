using UnityEngine;

public class StirDetector : MonoBehaviour
{
    [Header("Instellingen")]
    public string rodTag = "StirRod";
    public float requiredStirTime = 2.5f;
    public float minimumSpeed = 0.1f;

    private float currentStirTime = 0f;
    private bool isStirringComplete = false;

    private void OnTriggerStay(Collider other)
    {
        if (isStirringComplete) return;

        if (other.CompareTag(rodTag))
        {
            Rigidbody rodRb = other.attachedRigidbody;
            
            if (rodRb != null && rodRb.linearVelocity.magnitude > minimumSpeed)
            {
                currentStirTime += Time.deltaTime;

                if (currentStirTime >= requiredStirTime)
                {
                    isStirringComplete = true;
                    Debug.Log("<color=green>[StirDetector]</color> Roeren voltooid! Signaal 'StirSolution' wordt nu naar de manager gestuurd.");
                    
                    if (UniversalProcedureManager.Instance != null)
                    {
                        UniversalProcedureManager.Instance.OnActionTriggered("StirSolution");
                    }
                }
            }
        }
    }

    // Roep deze aan wanneer de speler terugkeert naar het hoofdmenu of de beker leeggooit
    public void ResetStirring()
    {
        currentStirTime = 0f;
        isStirringComplete = false;
        Debug.Log("<color=white>[StirDetector]</color> Detector is gereset en klaar voor een nieuwe sessie.");
    }
}