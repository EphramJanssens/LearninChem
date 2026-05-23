using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BuretValve : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    private bool isOpen = false;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        // Luister naar de klik van de speler
        interactable.selectEntered.AddListener(ToggleValve);
    }

    private void ToggleValve(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        isOpen = !isOpen; // Wissel de status

        if (isOpen)
        {
            // Draai het model 90 graden open (visuele feedback)
            transform.localEulerAngles = new Vector3(0, 0, 90);
            UniversalProcedureManager.Instance.OnActionTriggered("OpenValve");
        }
        else
        {
            // Draai het model terug dicht
            transform.localEulerAngles = new Vector3(0, 0, 0);
            UniversalProcedureManager.Instance.OnActionTriggered("CloseValve");
        }
    }
}
