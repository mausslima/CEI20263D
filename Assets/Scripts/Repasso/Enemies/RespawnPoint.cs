using Unity.VisualScripting;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("Rate")]
    [SerializeField] float spawnRate = 4f;
    [SerializeField] int maxEnemies = 3;

    private int enemiesAlive;
    private float nextSpawn;

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive >= maxEnemies) return;

        if (Time.time < nextSpawn) return;

        EnemySpawn();
        nextSpawn = Time.time + spawnRate;
    }

    private void EnemySpawn()
    {
        if (enemyPrefab == null) return;

        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        EnemyLife enemyLife = newEnemy.GetComponent<EnemyLife>();
        if (enemyLife != null) enemyLife.SetSpawner(this);

        enemiesAlive++;
    }
    public void DeadEnemy()
    {
        enemiesAlive--;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
