using UnityEngine;

public class PoolBulletsPlayer : MonoBehaviour
{
    [SerializeField] int poolSize = 20;
    [SerializeField] GameObject bulletPrefab;

    private GameObject[] bullets;
    private int nextBullet = -1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // All the bullets are created when starting, but disabled and out of view, ready to be pulled/recicled
    void Start()
    {
        bullets = new GameObject[poolSize];

        for ( int i = 0; i < poolSize; i++ )
        {
            bullets[i] = Instantiate( bulletPrefab, new Vector3(0f, 10000f, 0f ), Quaternion.identity);
            bullets[i].SetActive( false );
        }
    }

    public void BulletShoot(Vector3 position, Quaternion rotation)
    {
        //advance the pointer to grab the bullets and reset the counter at the end of the pool
        nextBullet++;
        if (nextBullet > poolSize - 1) nextBullet = 0;

        // put the recicled bullet in the shootiong point and activate it
        bullets[nextBullet].transform.position = position;
        bullets[nextBullet].transform.rotation = rotation;
        bullets[nextBullet].SetActive( true );
    }
}
