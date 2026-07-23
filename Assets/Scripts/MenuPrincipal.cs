using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] RectTransform panelFade;
    [SerializeField] GameObject contenedor1;
    [SerializeField] GameObject contenedor2;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text texto;

    //AudioSource _AudioSource;

    Color c;
    bool final;
    float valorVolume;
    string nombreJugador;

    private void Start()
    {
        contenedor2.gameObject.SetActive(false);
        c = panelFade.gameObject.GetComponent<Image>().color;
        StartCoroutine(FadeIn());
        texto.text = "Version: " + Application.version;
    }

    //Llamada evento boton Play
    public void MenuStart()
    {
        final = true;
        StartCoroutine(FadeOut());
    }

    //Llamada evento boton logros
    public void MenuLogros()
    {
        StartCoroutine(FadeOut());
        contenedor1.gameObject.SetActive(false);
        contenedor2.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    //Llamada evento boton back logros
    public void BackLogros()
    {
        StartCoroutine(FadeOut());
        contenedor2.gameObject.SetActive(false);
        contenedor1.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    //Llamada evento imput nombre jugador
    public void NombreJugador()
    {
        nombreJugador = inputField.text;
        Debug.Log("Nombre jugador: " + nombreJugador);
    }

    //Llamada evento slider volumen
    public void Volumen()
    {
        valorVolume = slider.value;
        //_AudioSource.volume = valorVolume;
        Debug.Log("Valor volume: " + valorVolume);
    }

    //Alpha out panel In
    IEnumerator FadeIn()
    {
        for (float Alfa = 1f; Alfa >= 0; Alfa -= 0.05f)
        {
            c.a = Alfa;
            panelFade.gameObject.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.01f);
        }
        c.a = 0f;
        panelFade.gameObject.GetComponent<Image>().color = c;
        yield return new WaitForSeconds(0.1f);
    }

    //Alpha panel Out
    IEnumerator FadeOut()
    {
        for (float Alfa = 0f; Alfa <= 1; Alfa += 0.05f)
        {
            c.a = Alfa;
            panelFade.gameObject.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.01f);
        }
        c.a = 1f;
        panelFade.gameObject.GetComponent<Image>().color = c;
        yield return new WaitForSeconds(0.00001f);
        if (final) SceneManager.LoadScene("Player CharacterController");
    }
}
