using UnityEngine;

public class ShootControl : MonoBehaviour
{
    InputControl inputControl;

    PlayerControl playerControl;

    [SerializeField] PoolBulletsPlayer bulletsPool;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float shootRate = 0.2f;
    [SerializeField] AudioSource shootAudio;

    private float nextBullet;

    //private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputControl = GetComponent<InputControl>();
        //characterController = GetComponent<CharacterController>();
        playerControl = GetComponent<PlayerControl>();
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, playerControl.rotationSpeed * Time.deltaTime);
        }

        Shoot();

    }

    private void Shoot()
    {
        if (!inputControl.isShooting) return;
        if (Time.time < nextBullet) return;
        if (bulletsPool == null || shootingPoint == null) return;

        bulletsPool.BulletShoot(shootingPoint.position, shootingPoint.rotation);
        if (shootAudio != null) shootAudio.Play();
        nextBullet = Time.time + shootRate;
    }
}
