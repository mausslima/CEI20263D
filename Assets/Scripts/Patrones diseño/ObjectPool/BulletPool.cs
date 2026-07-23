using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] int BulletPoolSize; //Tamaño de la array de objetos a reciclar
    [SerializeField] GameObject Bullet; //Prefab bala
    private GameObject[] _Bullets; //Array de balas
    private int ShootNumber = -1; //Número con la posición del array que toca activar y gestionar

void Start()
    {
        //Creamos la array con un tamaño igual al de la variable "BulletPoolSize"
        _Bullets = new GameObject[BulletPoolSize];
        //De forma secuencial, gracias a un bucle for creamos todas las balas, recordar
        //que estas comienzan desactivadas con lo cual no se ejecutará su script y
        //saldrán todas disparadas a la vez. También fijaos en la posición de creación:
        // el valor 1000 de la “y” hace que estén fuera de la escena por seguridad.
        for (int i = 0; i < BulletPoolSize; i++)
        {
            _Bullets[i] = Instantiate(Bullet, new Vector3(0f, 1000f, 0f), Quaternion.identity);
            _Bullets[i].SetActive(false);
        }
    }

    public void ShootBullet()
    {
        //Cada vez que disparemos, el “puntero” del array aumenta en uno para que
        //en el siguiente disparo señale a la siguiente bala del array.
        ShootNumber++;
        //En el caso de que el puntero supere el número de posiciones del array
        //vuelve a 0 para seguir con el proceso.
        if (ShootNumber > BulletPoolSize -1)
        {
            ShootNumber = 0;
        }
        //Ponemos la bala, desactivada aún, en la posición inicial
        _Bullets[ShootNumber].transform.position = new Vector3(0, 0, 0);
        //¡Activamos la bala!
        _Bullets[ShootNumber].SetActive(true);
    }
}
