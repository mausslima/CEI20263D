using UnityEngine;

public class ShootController : MonoBehaviour
{
    //Pasamos por el editor el objeto de la jerarquía que tiene el Script de "Bulletpool"
    [SerializeField] GameObject _BulletPool;

    void Update()
    {
        //Cuando precionamos el espacio, llamamos al método de Script "Bulletpool"
        if (Input.GetKeyDown(KeyCode.Space)) _BulletPool.GetComponent<BulletPool>().ShootBullet();
    }
}
