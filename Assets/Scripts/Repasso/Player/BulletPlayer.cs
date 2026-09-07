using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 30f;
    public float maxDistance = 100f;
    public float damage = 10f;

    private Vector3 shootPosition;

    private void OnEnable()
    {
        shootPosition = transform.position;
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceShot = Vector3.Distance(transform.position, shootPosition);

        if (distanceShot > maxDistance) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        //if (enemyLife != null)
        //{
        //    enemyLife.TakeDamage(damage);
        //    gameObject.SetActive(false);
        //}
    }
}
