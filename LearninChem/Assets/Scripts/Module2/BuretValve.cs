using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BuretValve : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    private bool isOpen = false;
    private Coroutine turnCoroutine;

    [Header("Instellingen")]
    [Tooltip("Hoe snel de kraan opendraait. Hoger is sneller.")]
    public float turnSpeed = 5f; 
    
    [Tooltip("Hoeveel seconden de speler moet wachten voor ze weer mogen klikken")]
    public float clickCooldown = 0.5f;
    private float lastClickTime = 0f;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(ToggleValve);
    }

    private void ToggleValve(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (Time.time - lastClickTime < clickCooldown)
        {
            Debug.Log("<color=grey>[BuretValve]</color> Te snel geklikt! Klik genegeerd ter bescherming.");
            return; 
        }

        lastClickTime = Time.time;
        
        isOpen = !isOpen; 
        
        Debug.Log($"<color=yellow>[BuretValve]</color> Kraan getriggert! Status is nu: {(isOpen ? "OPEN" : "DICHT")}");

        if (turnCoroutine != null)
        {
            StopCoroutine(turnCoroutine);
        }

        if (isOpen)
        {
            turnCoroutine = StartCoroutine(SmoothTurn(new Vector3(90, 0, 0)));
            UniversalProcedureManager.Instance.OnActionTriggered("OpenValve");
            
            if (TitrationController.Instance != null) 
            {
                TitrationController.Instance.StartTitration();
            }
        }
        else
        {
            turnCoroutine = StartCoroutine(SmoothTurn(Vector3.zero));
            UniversalProcedureManager.Instance.OnActionTriggered("CloseValve");
            
            if (TitrationController.Instance != null) 
            {
                TitrationController.Instance.StopTitration();
            }
        }
    }

    private IEnumerator SmoothTurn(Vector3 targetEulerAngles)
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * turnSpeed;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, progress);
            yield return null; 
        }

        transform.localRotation = targetRotation;
    }
}