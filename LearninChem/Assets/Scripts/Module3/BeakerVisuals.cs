using System.Collections;
using UnityEngine;

public class BeakerVisuals : MonoBehaviour
{
    public static BeakerVisuals Instance;

    [Header("Gekoppelde Renderer")]
    public Renderer liquidRenderer;

    [Header("Kleur Instellingen")]
    public Color leegKleur = new Color(1f, 1f, 1f, 0f);
    public Color kalkKleur = new Color(0.9f, 0.9f, 0.85f, 0.9f);
    public Color verdundeKleur = new Color(0.9f, 0.9f, 0.85f, 0.3f);

    [Header("Animatie Snelheid")]
    public float overgangsSnelheid = 2f;

    private Coroutine kleurAnimatie;

    void Awake()
    {
        Instance = this;
        if (liquidRenderer != null)
        {
            liquidRenderer.material.color = leegKleur;
        }
    }

    public void ToonKalk()
    {
        if (kleurAnimatie != null) StopCoroutine(kleurAnimatie);
        kleurAnimatie = StartCoroutine(VloeiendeKleur(kalkKleur));
    }

    public void ToonVerdund()
    {
        if (kleurAnimatie != null) StopCoroutine(kleurAnimatie);
        kleurAnimatie = StartCoroutine(VloeiendeKleur(verdundeKleur));
    }

    public void ResetBeker()
    {
        if (kleurAnimatie != null) StopCoroutine(kleurAnimatie);
        if (liquidRenderer != null) liquidRenderer.material.color = leegKleur;
    }

    private IEnumerator VloeiendeKleur(Color doelKleur)
    {
        Color startKleur = liquidRenderer.material.color;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * overgangsSnelheid;
            liquidRenderer.material.color = Color.Lerp(startKleur, doelKleur, progress);
            yield return null;
        }
        
        liquidRenderer.material.color = doelKleur;
    }
}