using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;  // reference to the input actions we generated (project right click -> create -> input actions)

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();  // input actions always need to be enabled to function
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
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
}
