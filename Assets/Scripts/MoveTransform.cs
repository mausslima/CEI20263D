using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    //Velocidad de movimiento del jugador
    float velPlayer = 20f;
    //Referencia al script EntradasPlayer
    EntradasPlayer entradasPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Obtenemos la referencia al script EntradasPlayer
        entradasPlayer = GetComponent<EntradasPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento con transform e Input Viejo
        //if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * velPlayer * Time.deltaTime);
        //if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.forward * -velPlayer * Time.deltaTime);
        //if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.right * -velPlayer * Time.deltaTime);
        //if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * velPlayer * Time.deltaTime);

        //Movimiento con transform e Input nuevo
        transform.Translate(entradasPlayer.movPlayer.x * velPlayer * Time.deltaTime, 0, entradasPlayer.movPlayer.y * velPlayer * Time.deltaTime);
    }
}
