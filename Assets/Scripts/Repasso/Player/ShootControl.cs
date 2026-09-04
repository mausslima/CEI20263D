using UnityEngine;

public class ShootControl : MonoBehaviour
{
    InputControl inputControl;
    private CharacterController characterController;
    private float verticalVelocity;
    private float rotationSpeed = 720f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputControl = GetComponent<InputControl>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(inputControl.playerShoot.x, 0f, inputControl.playerShoot.y);
        if (direction.sqrMagnitude > 1f) direction.Normalize();

        //Rotacao
        if (direction.sqrMagnitude > 0.1f && inputControl.isShooting)
        {
            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

    }
}
