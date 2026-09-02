using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = false;

    public float CurrentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0f);

        if (CurrentHealth < 0f) Die();
    }

    public void Heal (float amount)
    {
        if (IsDead) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
    }

    private void Die()
    {
        IsDead = true;

        Debug.Log("Morreu");

        if (destroyOnDeath) Destroy(gameObject);
        else
        {
            // bloqueia o movimento do personagem sem destruir o sem prefab
            PlayerMovement movement = GetComponent<PlayerMovement>();

            if (movement != null) movement.enabled = false;
        }
    }
}
