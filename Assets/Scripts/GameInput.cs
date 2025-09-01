using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;  // reference to the input actions we generated (project right click -> create -> input actions)

    public event EventHandler OnMenuButtonPressed;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();  // input actions always need to be enabled to function

        inputActions.Player.Menu.performed += Menu_performed;  // listening to menu performed; "esc" key pressed
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    public bool IsUpActionsPressed()
    {
        return inputActions.Player.LanderUp.IsPressed();
    }
    public bool IsLeftActionsPressed()
    {
        return inputActions.Player.LanderLeft.IsPressed();
    }
    public bool IsRightActionsPressed()
    {
        return inputActions.Player.LanderRight.IsPressed();
    }

    public Vector2 GetMovementInputVector2()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();  // do this to read the value of the Vector2 types
    }
}
