using UnityEngine;

public class MoverRB : MonoBehaviour
{
    //Velocidad de movimiento del jugador
    float velPlayer = 20f;
    //Referencia al script EntradasPlayer
    EntradasPlayer entradasPlayer;
    //Referencia al Rigidbody del jugador
    Rigidbody rb;
    //Fuerza de salto del jugador
    float fuerzaSalto = 50f;

    PlayerJump playerJumpRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Obtenemos la referencia al script EntradasPlayer
        entradasPlayer = GetComponent<EntradasPlayer>();
        //Obtenemos la referencia al Rigidbody del jugador
        entradasPlayer.superCorrer = 1f;
        //Obtenemos la referencia al Rigidbody del jugador
        rb = GetComponent<Rigidbody>();

        playerJumpRB = GetComponent<PlayerJump>();
    }

    private void FixedUpdate()
    {
        //Movimiento con el Rigidbody a través del input nuevo
        rb.linearVelocity = new Vector3(entradasPlayer.movPlayer.x * velPlayer * entradasPlayer.superCorrer, playerJumpRB.velocity.y, entradasPlayer.movPlayer.y * velPlayer * entradasPlayer.superCorrer);

        //Aplicamos una fuerza hacia arriba al Rigidbody del jugador para que salte
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        //Desactivamos el salto para que no se repita hasta qie se vuelva a pulsar la tecla de salto
    }
}


