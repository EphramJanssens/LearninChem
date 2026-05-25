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
                    Debug.Log("<color=green>[StirDetector]</color> Roeren voltooid!");
                    
                    if (UniversalProcedureManager.Instance != null)
                    {
                        UniversalProcedureManager.Instance.OnActionTriggered("StirSolution");
                    }
                }
            }
            else
            {
            }
        }
    }
}