using UnityEngine;
using UnityEngine.UI;

public class UIPlayerLife : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Slider lifeBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeBar.maxValue = playerHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        lifeBar.value = playerHealth.currentHealth;
    }
}
