using UnityEngine;
using UnityEngine.InputSystem;

public class EntradasPlayer : MonoBehaviour
{
    //Referencia al script PlayerInput
    private PlayerInput playerInput;
    //Vector2 que almacena el movimiento del jugador
    public Vector2 movPlayer;
    //Float que almacena la velocidad de supercorrer del jugador
    public float superCorrer;

    public PlayerJump playerJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Obtenemos la referencia al script PlayerInput
        playerInput = GetComponent<PlayerInput>();
        playerJump = GetComponent<PlayerJump>();
    }

    //Método que se ejecuta cuando el jugador pulsa la tecla de movimiento
    public void Andar(InputAction.CallbackContext callbackContext)
    {
        //Si el callbackContext es performed, almacenamos el valor del movimiento del jugador en movPlayer
        if (callbackContext.performed)
        {
            //Almacenamos el valor del movimiento del jugador en movPlayer
            movPlayer = playerInput.actions["Andar"].ReadValue<Vector2>();
        }

        //Si el callbackContext es canceled, almacenamos el valor del movimiento del jugador en movPlayer como Vector2.zero
        if (callbackContext.canceled)
        {
            //Almacenamos el valor del movimiento del jugador en movPlayer como Vector2.zero
            movPlayer = Vector2.zero;
        }
    }

    //Método que se ejecuta cuando el jugador pulsa la tecla de supercorrer
    public void Correr(InputAction.CallbackContext callbackContext)
    {
        //Si el callbackContext es performed, almacenamos el valor de superCorrer como 2f
        if (callbackContext.performed)
        {
            //Almacenamos el valor de superCorrer como 2f
            superCorrer = 2f;
        }
        else
        //Si el callbackContext es canceled, almacenamos el valor de superCorrer como 1f
        if (callbackContext.canceled)
        {
            //Almacenamos el valor de superCorrer como 1f
            superCorrer = 1f;
        }
    }

    //Método que se ejecuta cuando el jugador pulsa la tecla de salto
    public void Salto(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerJump.jumpBufferCounter = playerJump.jumpBufferTime;
            playerJump.jumpCancel = false;
        }
        else
        {
            if (callbackContext.canceled)
            {
                playerJump.jumpCancel = true;
                playerJump.coyoteTimeCounter = 0f;
            }
        }
    }
}
