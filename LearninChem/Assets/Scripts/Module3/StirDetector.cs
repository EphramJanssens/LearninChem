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
                    
                    if (ConductivityMeter.Instance != null)
                    {
                        ConductivityMeter.Instance.isSolutionStirred = true;
                        Debug.Log("<color=cyan>[StirDetector]</color> Geleidbaarheidsmeter is nu ontgrendeld.");
                    }
                    
                    if (UniversalProcedureManager.Instance != null)
                    {
                        UniversalProcedureManager.Instance.OnActionTriggered("StirSolution");
                    }
                }
            }
        }
    }

    public void ResetStirring()
    {
        currentStirTime = 0f;
        isStirringComplete = false;
        Debug.Log("<color=white>[StirDetector]</color> Detector is gereset en klaar voor een nieuwe sessie.");
    }
}