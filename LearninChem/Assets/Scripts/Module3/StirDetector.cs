using UnityEngine;

public class StirDetector : MonoBehaviour
{
    [Header("Instellingen")]
    public string rodTag = "StirRod";
    public float requiredStirTime = 2.5f; // Aantal seconden dat de speler moet roeren
    public float minimumSpeed = 0.1f; // Gevoeligheid: hoe hard moeten ze roeren?

    private float currentStirTime = 0f;
    private bool isStirringComplete = false;

    private void OnTriggerStay(Collider other)
    {
        // Als we al klaar zijn, hoeven we niets meer te doen
        if (isStirringComplete) return;

        // Check of het binnendringende object onze roerstaaf is
        if (other.CompareTag(rodTag))
        {
            Rigidbody rodRb = other.attachedRigidbody;
            
            // Check of de staaf een Rigidbody heeft én snel genoeg beweegt
            // (linearVelocity is de correcte API voor Unity 6)
            if (rodRb != null && rodRb.linearVelocity.magnitude > minimumSpeed)
            {
                currentStirTime += Time.deltaTime;
                
                // Optionele log om te zien of hij registreert tijdens het testen
                Debug.Log($"<color=yellow>[StirDetector]</color> Aan het roeren... {currentStirTime:F1} / {requiredStirTime} sec");

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
                // Zakt de snelheid onder het minimum? Dan roeren ze niet (houden hem stil).
                // Je kunt er hier voor kiezen om de timer langzaam leeg te laten lopen,
                // maar voor nu laten we hem gewoon pauzeren.
            }
        }
    }
}