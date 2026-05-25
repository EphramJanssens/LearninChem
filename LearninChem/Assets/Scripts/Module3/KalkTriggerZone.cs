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
                Debug.Log("<color=cyan>[KalkTrigger]</color> Kalk gedetecteerd! Update de weegschaal.");
                targetScale.AddKalk();
                hasAddedKalk = true;
            }
        }
    }

    public void ResetTrigger()
    {
        hasAddedKalk = false;
    }
}