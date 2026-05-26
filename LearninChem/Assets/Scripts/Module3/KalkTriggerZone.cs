using UnityEngine;

public class KalkTriggerZone : MonoBehaviour
{
    [Header("Koppelingen")]
    public WeighingScale targetScale;
    public string kalkTag = "KalkBottle";

    private bool hasAddedKalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasAddedKalk) return;

        if (other.CompareTag(kalkTag))
        {
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
                    Debug.Log("<color=yellow>[KalkTrigger]</color> Toevoegen geweigerd. Trigger blijft open voor een nieuwe poging.");
                }
            }
        }
    }

    public void ResetTrigger()
    {
        hasAddedKalk = false;
    }
}