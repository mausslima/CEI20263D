using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;
    [SerializeField] private string playerTag = "Player";

    [Header("Apontar")]
    [SerializeField] private Transform rotatingPart;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private bool rotateOnlyOnYAxis = true;

    [Header("Atirar")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float attackRange = 20f;
    [SerializeField] private float firstShotDelay = 0.5f;

    [Header("Linha de visao")]
    [SerializeField] private bool requireLineOfSight = true;
    [SerializeField] private LayerMask lineOfSightMask = ~0;

    private float nextShotTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        FindPlayerIfNeeded();
        nextShotTime = Time.time + firstShotDelay;
    }

    // Update is called once per frame
    private void Update()
    {
        FindPlayerIfNeeded();

        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > attackRange) return;

        RotateTowardsTarget();

        if (Time.time >= nextShotTime && CanShootTarget())
        {
            Shoot();
            nextShotTime = Time.time + 1f / fireRate;
        }
    }

    private void FindPlayerIfNeeded()
    {
        if (target != null) return;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null) target = player.transform;
    }

    private void RotateTowardsTarget()
    {
        Transform partToRotate = rotatingPart != null ? rotatingPart : transform;

        Vector3 directionToTarget = target.position - partToRotate.position;

        if (rotateOnlyOnYAxis) directionToTarget.y = 0f;

        if (directionToTarget.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

        partToRotate.rotation = Quaternion.RotateTowards(
            partToRotate.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    private bool CanShootTarget()
    {
        if (!requireLineOfSight) return true;
        if (firePoint == null) return false;

        Vector3 origin = firePoint.position;
        Vector3 direction = (target.position - origin).normalized;
        float distanceToTarget = Vector3.Distance(origin, target.position);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distanceToTarget, lineOfSightMask))
        {
            return hit.transform == target || hit.transform.IsChildOf(target);
        }

        return false;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.Log("Faltam referencias a torre: prefab do projetil ou firepoint");
            return;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation);

        TurretProjectile projectile = projectileObject.GetComponent<TurretProjectile>();

        if (projectile != null) projectile.SetOwner(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (firePoint != null && target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(firePoint.position, target.position);
        }
    }
}
