using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 verticalVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 moveDirection = inputDirection;

        // se houver camera, o movimento se adapta a sua orientacao horizontal
        if  (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // o personagem gira olhando para a direcao que esta caminhando
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }
}
