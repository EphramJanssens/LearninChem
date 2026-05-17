using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class VRButtonTester : MonoBehaviour
{
    private Button _button;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Find the button by its UXML name
        _button = root.Q<Button>("StartTrainingBtn");

        if (_button != null)
        {
            // Use Pointer events because XR Interactors simulate pointer positions
            _button.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            _button.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            _button.RegisterCallback<ClickEvent>(OnButtonClick);
            
            Debug.Log("<color=white><b>[VR Tester]:</b></color> Found 'StartTrainingBtn' successfully. Ready for VR input.");
        }
        else
        {
            Debug.LogError("[VR Tester]: Could not find a button named 'StartTrainingBtn'. Check your UXML names!");
        }
    }

    void OnDisable()
    {
        if (_button != null)
        {
            _button.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            _button.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            _button.UnregisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnPointerEnter(PointerEnterEvent evt)
    {
        Debug.Log("<color=green><b>[VR UI HOVER]:</b></color> Ray/Hand entered the button boundary!");
    }

    private void OnPointerLeave(PointerLeaveEvent evt)
    {
        Debug.Log("<color=yellow><b>[VR UI LEAVE]:</b></color> Ray/Hand left the button boundary.");
    }

    private void OnButtonClick(ClickEvent evt)
    {
        Debug.Log("<color=cyan><b>[VR UI CLICK]:</b></color> Button clicked successfully!");
    }
}