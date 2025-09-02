using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TouchUI : MonoBehaviour
{
    [SerializeField] private GameObject touchButtonsGroup, joystick;

    [SerializeField] private Button touchControlButton;
    [SerializeField] private TextMeshProUGUI touchControlText;

    private enum ControlMethod
    {
        NoTouch,
        TouchButtons,
        TouchStick,
    };

    private ControlMethod controlMethod = ControlMethod.NoTouch;

    private void Awake()
    {
        if (controlMethod == ControlMethod.NoTouch)
        {
            touchButtonsGroup.SetActive(false);
            joystick.SetActive(false);
            touchControlText.text = "NO TOUCH";
        }

        touchControlButton.onClick.AddListener(() =>
        {
            UpdateTouchUI();
        });
    }


    private void UpdateTouchUI()
    {
        // Cycle to next control method
        controlMethod = (ControlMethod)(((int)controlMethod + 1) % System.Enum.GetValues(typeof(ControlMethod)).Length);
        ToggleControlMethod();
    }

    private void ToggleControlMethod()
    {
        switch (controlMethod)
        {
            case ControlMethod.NoTouch:
                touchButtonsGroup.SetActive(false);
                joystick.SetActive(false);
                touchControlText.text = "NO TOUCH";
                break;
            case ControlMethod.TouchButtons:
                touchButtonsGroup.SetActive(true);
                joystick.SetActive(false);
                touchControlText.text = "BUTTONS";
                break;
            case ControlMethod.TouchStick:
                touchButtonsGroup.SetActive(false);
                joystick.SetActive(true);
                touchControlText.text = "JOYSTICK";
                break;
            default:
                break;
        }
    }
}
