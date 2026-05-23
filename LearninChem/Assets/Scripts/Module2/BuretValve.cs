using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BuretValve : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    private bool isOpen = false;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(ToggleValve);
    }

    private void ToggleValve(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        isOpen = !isOpen; 
        
        Debug.Log($"<color=yellow>[BuretValve]</color> Kraan is aangeklikt! Status is nu: {(isOpen ? "OPEN" : "DICHT")}");

        if (isOpen)
        {
            transform.localEulerAngles = new Vector3(0, 0, 90);
            UniversalProcedureManager.Instance.OnActionTriggered("OpenValve");
            
            // CHECK 2: Bestaat de controller of is hij kwijt?
            if (TitrationController.Instance != null) 
            {
                Debug.Log("<color=yellow>[BuretValve]</color> Controller gevonden, ik roep StartTitration() aan!");
                TitrationController.Instance.StartTitration();
            }
            else
            {
                Debug.LogError("<color=red>[BuretValve]</color> OEPS! TitrationController.Instance is NULL. Staat het script in je scene?");
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            UniversalProcedureManager.Instance.OnActionTriggered("CloseValve");
            
            if (TitrationController.Instance != null) TitrationController.Instance.StopTitration();
        }
    }
}