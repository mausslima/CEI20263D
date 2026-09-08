using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    InputControl inputControl;
    private CharacterController characterController;

    private float speed = 15f;
    public float rotationSpeed = 720f;
    private float gravity = -20f;

    private float jumpForce = 10f;
    [SerializeField] AudioSource jumpAudio;

    private float verticalVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputControl = GetComponent<InputControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(inputControl.inputPlayer.x, 0f, inputControl.inputPlayer.y);
        if (direction.sqrMagnitude > 1f ) direction.Normalize();

        //Rotacao
        if (direction.sqrMagnitude > 0.1f && !inputControl.isShooting)
        {
            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        //Movimento player
        if (!characterController.isGrounded) { if (verticalVelocity > -50) verticalVelocity += gravity * Time.deltaTime; }
        else verticalVelocity = -10f;

        if (characterController.isGrounded && inputControl.inputJump)
        {
            verticalVelocity = jumpForce;
            if (jumpAudio != null) jumpAudio.Play();
        }

        Vector3 velocity = direction * speed;
        velocity.y = verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }
}
