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

        Debug.Log($"<color=orange>[ResetSysteem]</color> {gameObject.name} heeft startpositie onthouden: {startPosition}");
    }

    public void ResetToHome()
    {
        StartCoroutine(ResetSequence());
    }

    private IEnumerator ResetSequence()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

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

        transform.position = startPosition;
        transform.rotation = startRotation;

        yield return null;

        if (rb != null)
        {
            rb.isKinematic = originalIsKinematic;
        }
        
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }

        Debug.Log($"<color=orange>[ResetSysteem]</color> {gameObject.name} is succesvol gereset naar de tafel!");
    }
}