using UnityEngine;

/*
 * Functie: Detecteert wanneer de kalkfles de zone van de weegschaal raakt en vraagt toestemming om kalk toe te voegen, inclusief anti spam.
 * Invloed: Roept direct de AddKalk() functie aan op het gekoppelde WeighingScale script.
 */

public class KalkTriggerZone : MonoBehaviour
{
    [Header("Koppelingen")]
    public WeighingScale targetScale;
    public string kalkTag = "KalkBottle";

    [Header("Anti-Spam Instellingen")]
    [Tooltip("Hoeveel seconden de trigger pauzeert na een mislukte poging.")]
    public float triggerCooldown = 2.0f;
    
    private float lastTriggerTime = -10f;
    private bool hasAddedKalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasAddedKalk) return;

        if (other.CompareTag(kalkTag))
        {
            if (Time.time - lastTriggerTime < triggerCooldown)
            {
                return; 
            }

            lastTriggerTime = Time.time;

            if (targetScale != null)
            {
                Debug.Log("<color=cyan>[KalkTrigger]</color> Kalk gedetecteerd! Vraag aan de weegschaal of dit mag...");
                
                bool isSuccesvol = targetScale.AddKalk();
                
                if (isSuccesvol)
                {
                    hasAddedKalk = true;
                }
                else
                {
                    Debug.Log("<color=yellow>[KalkTrigger]</color> Toevoegen geweigerd. Trigger pauzeert nu voor " + triggerCooldown + " seconden.");
                }
            }
        }
    }

    public void ResetTrigger()
    {
        hasAddedKalk = false;
        lastTriggerTime = -10f;
    }
}