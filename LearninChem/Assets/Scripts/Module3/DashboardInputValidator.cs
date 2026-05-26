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

    private Color darkTextColor = new Color(0.1f, 0.11f, 0.14f);

    void OnEnable()
    {
        if (dashboardUI != null && dashboardUI.rootVisualElement != null)
        {
            var root = dashboardUI.rootVisualElement;
            
            inputSection = root.Q<VisualElement>("InputSection");
            inputField = root.Q<TextField>("ConductivityInput");

            if (inputSection != null) inputSection.style.display = DisplayStyle.None;

            if (inputField != null)
            {
                inputField.style.color = new StyleColor(darkTextColor);
            }

            btnClear = root.Q<Button>("BtnClear");
            if (btnClear != null) btnClear.clicked += () => { if (inputField != null) inputField.value = ""; };

            btnEnter = root.Q<Button>("BtnEnter");
            if (btnEnter != null) btnEnter.clicked += OnSubmitClicked;

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
                
                if (inputSection != null) inputSection.style.display = DisplayStyle.None;
            }
            else
            {
                Debug.Log("<color=red>[Validator]</color> Foutieve waarde ingevoerd!");
                inputField.value = "";
            }
        }
    }

    public void ResetValidator()
    {
        if (inputField != null)
        {
            inputField.value = "";
            inputField.style.color = new StyleColor(darkTextColor); 
        }

        if (inputSection != null)
        {
            inputSection.style.display = DisplayStyle.None;
        }
    }
}