using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TouchUI : MonoBehaviour
{
    [SerializeField] private GameObject touchButtonsGroup, joystick;
    [SerializeField] private Button touchControlButton;
    [SerializeField] private TextMeshProUGUI touchControlText;

    private static ControlMethod controlMethod = ControlMethod.NoTouch;

    private void Awake()
    {
        touchControlButton.onClick.AddListener(() =>
        {
            UpdateTouchUI();
            Debug.Log("Pressed Control Method Button");
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;
        Lander.Instance.OnLanded += Lander_OnLanded;

        ToggleControlMethod();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        HideTouchUI();
    }

    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        ShowTouchUI();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        HideTouchUI();
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

    private void ShowTouchUI()
    {
        gameObject.SetActive(true);
    }

    private void HideTouchUI()
    {
        gameObject.SetActive(false);
    }
    
    //public ControlMethod GetControlMethod()
    //{
    //    return controlMethod;
    //}

    //public void SetControlMethod(ControlMethod newControlMethod)
    //{
    //    controlMethod = newControlMethod;
    //}
}
