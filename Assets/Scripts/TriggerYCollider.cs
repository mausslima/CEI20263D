using UnityEngine;

public class TriggerYCollider : MonoBehaviour
{
    //Metodo que se ejecuta cuando el objeto colisiona con otro objeto. Variantes OnCollisionEnter, OnCollisionStay y OnCollisionExit
    private void OnCollisionStay(Collision collision)
    {
        //Verificamos si el objeto con el que colisionamos tiene el tag "Obstaculo"
        if (collision.gameObject.tag == "Obstaculo")
        {
            //Si es asi, mostramos un mensaje en la consola
            Debug.Log("Colision con obstaculo");
        }
    }

    //Metodo que se ejecuta cuando el objeto entra en un trigger. Variantes OnTriggerEnter, OnTriggerStay y OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        //Verificamos si el objeto con el que entramos en el trigger tiene el tag "Obstaculo"
        if (other.gameObject.tag == "Obstaculo")
        {
            //Si es asi, mostramos un mensaje en la consola
            Debug.Log("Intersección con obstaculo");
        }
    }

    private void OnEnable()
    {
        //Mostramos un mensaje en la consola cuando el script se habilita
        Debug.Log("Script habilitado");
    }

    private void OnDisable()
    {
        //Mostramos un mensaje en la consola cuando el script se deshabilita
        Debug.Log("Script deshabilitado");
    }
}
