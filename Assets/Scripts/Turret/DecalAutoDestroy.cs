using UnityEngine;

public class DecalAutoDestroy : MonoBehaviour
{
    private float lifetime = 20f;

    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
