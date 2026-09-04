using UnityEngine;
using UnityEngine.InputSystem;

public class InputControl : MonoBehaviour
{
    public Vector2 inputPlayer;
    public Vector2 playerShoot;
    public bool isShooting = false;

    public void InputMove(InputAction.CallbackContext callbackContext)
    {
        switch (callbackContext.phase)
        {
            case InputActionPhase.Performed:
                inputPlayer = callbackContext.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                inputPlayer = Vector2.zero;
                break;
        }
    }

    public void InputShoot(InputAction.CallbackContext callbackContext)
    {
        switch (callbackContext.phase)
        {
            case InputActionPhase.Performed:
                playerShoot = callbackContext.ReadValue<Vector2>();
                isShooting = true;
                break;
            case InputActionPhase.Canceled:
                playerShoot = Vector2.zero;
                isShooting = false;
                break;
        }
    }
}
