using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Declaramos variable camara
    Camera _Cam;

    private void Start()
    {
        //Capturamos la cámara de la escena
        _Cam = Camera.main;
    }

    private void Update()
    {
        //Cuando la bala está lejos de nuestra cámara, se hubica fuera de la vista del juego y se desactiva
        float dist = Vector3.Distance(gameObject.transform.position, _Cam.transform.position);

        if (dist > 100f)
        {
            gameObject.SetActive(false);
            gameObject.transform.position = new Vector3(0f, 1000f, 0f);
        }
    }

    //Cuando se activa cada bala, se le aplica el movimiento
    private void OnEnable()
    {
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0f, 0f, 100f);
    }
}

