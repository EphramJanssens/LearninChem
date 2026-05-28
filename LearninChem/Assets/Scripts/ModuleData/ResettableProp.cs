using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ResettableProp : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private bool originalIsKinematic;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (rb != null)
        {
            originalIsKinematic = rb.isKinematic;
        }

        //Debug.Log($"<color=orange>[ResetSysteem]</color> {gameObject.name} heeft startpositie onthouden: {startPosition}");
    }

    public void ResetToHome()
    {
        StartCoroutine(ResetSequence());
    }

private IEnumerator ResetSequence()
    {
        // 1. Forceer het object om uit handen OF sockets te vallen
        if (grabInteractable != null)
        {
            // Door hem heel even uit en aan te zetten, breekt hij alle XR-verbindingen (zoals sockets)
            grabInteractable.enabled = false;
        }

        // Geef Unity XR één frame de tijd om de socket leeg te maken
        yield return null; 

        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
        }

        yield return null;

        // Nu is hij veilig los en kunnen we hem teleporteren
        transform.position = startPosition;
        transform.rotation = startRotation;

        yield return null;

        if (rb != null)
        {
            rb.isKinematic = originalIsKinematic;
        }
        
        // Zet de interactie weer aan voor de volgende ronde
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }
}