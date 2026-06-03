using UnityEngine;

/* 
 * Functie: Verandert het materiaal van de virtuele handen naar groene latex handschoenen zodra de speler de dispenser aanraakt.
 * Invloed: Stuurt een succes signaal (actionID) door naar de UniversalProcedureManager om de stap te voltooien en kan gereset worden door de manager.
 */

public class GloveDispenser : MonoBehaviour
{
    [Header("VR Hand Renderers")]
    public Renderer leftHandRenderer;
    public Renderer rightHandRenderer;

    [Header("Materials")]
    public Material greenLatexMaterial;
    
    private Material originalLeftMaterial;
    private Material originalRightMaterial;

    [Header("Manager Settings")]
    public string actionID = "EquipGloves";
    
    private bool glovesEquipped = false;

    void Awake()
    {
        if (leftHandRenderer != null) originalLeftMaterial = leftHandRenderer.material;
        if (rightHandRenderer != null) originalRightMaterial = rightHandRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (glovesEquipped) return;

        if (other.CompareTag("PlayerHand") || other.CompareTag("Player"))
        {
            EquipGloves();
        }
    }

    private void EquipGloves()
    {
        if (leftHandRenderer != null && greenLatexMaterial != null) leftHandRenderer.material = greenLatexMaterial;
        if (rightHandRenderer != null && greenLatexMaterial != null) rightHandRenderer.material = greenLatexMaterial;

        glovesEquipped = true;

        if (UniversalProcedureManager.Instance != null)
        {
            UniversalProcedureManager.Instance.OnActionTriggered(actionID);
        }
    }

    public void ResetGloves()
    {
        if (leftHandRenderer != null && originalLeftMaterial != null) leftHandRenderer.material = originalLeftMaterial;
        if (rightHandRenderer != null && originalRightMaterial != null) rightHandRenderer.material = originalRightMaterial;
        
        glovesEquipped = false;
    }
}