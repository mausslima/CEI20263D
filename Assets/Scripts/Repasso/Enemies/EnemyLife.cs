using UnityEngine;
using UnityEngine.AI;

public class EnemyLife : MonoBehaviour
{
    [Header("Life")]
    public float maxHealth = 30f;
    public float currentHealth;
    public bool isDead;

    private RespawnPoint spawner;

    [Header("Death")]
    [SerializeField] GameObject prefabExplosion;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void SetSpawner(RespawnPoint respawnPoint)
    {
        spawner = respawnPoint;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;

        EnemyAI chase = GetComponent<EnemyAI>();
        if (chase != null) chase.enabled = false;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        if (prefabExplosion != null)
        {
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        }

        if (spawner != null) spawner.DeadEnemy();

        Destroy(gameObject);
    }
}
