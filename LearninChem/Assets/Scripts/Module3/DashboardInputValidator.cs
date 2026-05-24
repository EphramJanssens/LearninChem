using UnityEngine;
using UnityEngine.UIElements;

public class DashboardInputValidator : MonoBehaviour
{
    [Header("UI Koppeling")]
    public UIDocument dashboardUI;

    private VisualElement inputSection;
    private TextField inputField;
    private Button btnEnter;
    private Button btnClear;
    private Button[] numButtons = new Button[10];

    void OnEnable()
    {
        if (dashboardUI != null && dashboardUI.rootVisualElement != null)
        {
            var root = dashboardUI.rootVisualElement;
            
            // Koppel de hoofd-container en het veld
            inputSection = root.Q<VisualElement>("InputSection");
            inputField = root.Q<TextField>("ConductivityInput");

            // Dubbele check: Forceer onzichtbaarheid bij de opstart
            if (inputSection != null) inputSection.style.display = DisplayStyle.None;

            // Koppel de C (Clear) en OK (Enter) knoppen
            btnClear = root.Q<Button>("BtnClear");
            if (btnClear != null) btnClear.clicked += () => { if (inputField != null) inputField.value = ""; };

            btnEnter = root.Q<Button>("BtnEnter");
            if (btnEnter != null) btnEnter.clicked += OnSubmitClicked;

            // Koppel de nummers 0 t/m 9
            for (int i = 0; i < 10; i++)
            {
                int number = i; 
                numButtons[i] = root.Q<Button>($"Btn{number}");
                if (numButtons[i] != null)
                {
                    numButtons[i].clicked += () => { if (inputField != null) inputField.value += number.ToString(); };
                }
            }
        }
    }

    void Update()
    {
        // Toon de hele Numpad-sectie pas zodra de meter een waarde heeft
        if (ConductivityMeter.Instance != null && ConductivityMeter.Instance.finalValue > 0)
        {
            if (inputSection != null && inputSection.style.display == DisplayStyle.None)
            {
                inputSection.style.display = DisplayStyle.Flex;
            }
        }
    }

    private void OnSubmitClicked()
    {
        if (ConductivityMeter.Instance == null || ConductivityMeter.Instance.finalValue == 0) return;

        if (int.TryParse(inputField.value, out int ingevoerdeWaarde))
        {
            int echteWaarde = ConductivityMeter.Instance.finalValue;

            if (ingevoerdeWaarde == echteWaarde)
            {
                Debug.Log("<color=green>[Validator]</color> Correcte waarde ingevoerd!");
                
                Color successColor = Color.green;
                ColorUtility.TryParseHtmlString("#2ECC71", out successColor);
                inputField.style.color = new StyleColor(successColor);
                
                if (UniversalProcedureManager.Instance != null)
                {
                    UniversalProcedureManager.Instance.OnActionTriggered("SubmitValue");
                }
                
                // Verberg het hele blok weer als het klaar is
                if (inputSection != null) inputSection.style.display = DisplayStyle.None;
            }
            else
            {
                Debug.Log("<color=red>[Validator]</color> Foutieve waarde ingevoerd!");
                inputField.value = ""; // Maak het veld leeg
            }
        }
    }
}