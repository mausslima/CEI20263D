using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerChaCon : MonoBehaviour
{
    [Header("Movimiento")]
    // Velocidad de movimiento del jugador
    [SerializeField] private float moveSpeed = 5f;

    EntradasPlayer entradasPlayer;
    PlayerJump playerJump;

    // Referencia al CharacterController del jugador
    private CharacterController controller;

    private void Awake()
    {
        // Obtener la referencia al CharacterController en el objeto del jugador
        controller = GetComponent<CharacterController>();
        entradasPlayer = GetComponent<EntradasPlayer>();
        playerJump = GetComponent<PlayerJump>();

        entradasPlayer.superCorrer = 1f; // Inicializar superCorrer a 1 para que el jugador se mueva a velocidad normal al inicio
    }

    private void Update()
    {
        // Calcular el movimiento final combinando la entrada horizontal y la velocidad vertical
        Vector3 finalMove = new Vector3(entradasPlayer.movPlayer.x, 0f, entradasPlayer.movPlayer.y) * moveSpeed * entradasPlayer.superCorrer;
        finalMove.y = playerJump.velocity.y;

        // Mover al jugador usando CharacterController.Move
        controller.Move(finalMove * Time.deltaTime);
    }
}