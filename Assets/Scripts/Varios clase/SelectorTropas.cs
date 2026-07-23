using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Este script controla:
// 1. La selección por arrastre con el ratón.
// 2. Dibujar el rectángulo de selección en pantalla.
// 3. Seleccionar las unidades que queden dentro del recuadro.
// 4. Mover las unidades seleccionadas al hacer clic en el suelo.
public class SelectorTropas : MonoBehaviour
{
    [Header("Referencias")]
    // Cámara principal desde la que convertimos posiciones mundo <-> pantalla.
    [SerializeField] private Camera camaraPrincipal;

    [Header("Capas")]
    // Layer del suelo para saber dónde hacer clic para mover unidades.
    [SerializeField] private LayerMask capaSuelo;

    [Header("Ajustes de selección")]
    // Distancia mínima en píxeles para considerar que ha habido arrastre.
    // Esto evita que un simple clic se interprete como selección en rectángulo.
    [SerializeField] private float distanciaMinimaArrastre = 10f;

    [Header("Dibujo del recuadro")]
    // Color semitransparente del recuadro de selección.
    [SerializeField] private Color colorRelleno = new Color(0f, 1f, 0f, 0.15f);

    // Color del borde del recuadro.
    [SerializeField] private Color colorBorde = Color.green;

    // Posición en pantalla donde empezó el clic.
    private Vector2 posicionInicialRaton;

    // Posición actual del ratón mientras arrastramos.
    private Vector2 posicionActualRaton;

    // Indica si estamos arrastrando para seleccionar.
    private bool arrastrandoSeleccion = false;

    // Lista de todas las unidades que existen en la escena.
    private UnidadSeleccionable[] todasLasUnidades;

    // Lista de las unidades actualmente seleccionadas.
    private List<UnidadSeleccionable> unidadesSeleccionadas = new List<UnidadSeleccionable>();

    // Textura de 1x1 para dibujar el rectángulo en OnGUI.
    private Texture2D texturaBlanca;

    private void Awake()
    {
        // Si no asignamos cámara en el Inspector, usamos la principal.
        if (camaraPrincipal == null)
        {
            camaraPrincipal = Camera.main;
        }

        // Creamos una textura blanca de 1x1 para dibujar el recuadro.
        texturaBlanca = new Texture2D(1, 1);
        texturaBlanca.SetPixel(0, 0, Color.white);
        texturaBlanca.Apply();
    }

    private void Start()
    {
        // Buscamos todas las unidades seleccionables de la escena al empezar.
        todasLasUnidades = FindObjectsByType<UnidadSeleccionable>();
    }

    private void Update()
    {
        GestionarSeleccionPorArrastre();
        GestionarMovimientoDeSeleccionados();
    }

    /// <summary>
    /// Gestiona la lógica de clicar, arrastrar y soltar para seleccionar unidades.
    /// </summary>
    private void GestionarSeleccionPorArrastre()
    {
        // Al pulsar botón izquierdo, guardamos la posición inicial.
        if (Input.GetMouseButtonDown(0))
        {
            posicionInicialRaton = Input.mousePosition;
            posicionActualRaton = Input.mousePosition;
            arrastrandoSeleccion = false;
        }

        // Mientras mantenemos el botón izquierdo, actualizamos la posición actual.
        if (Input.GetMouseButton(0))
        {
            posicionActualRaton = Input.mousePosition;

            // Si el ratón se ha movido lo suficiente, consideramos que estamos arrastrando.
            if (Vector2.Distance(posicionInicialRaton, posicionActualRaton) > distanciaMinimaArrastre)
            {
                arrastrandoSeleccion = true;
            }
        }

        // Al soltar el botón izquierdo, si estábamos arrastrando,
        // hacemos la selección de unidades dentro del rectángulo.
        if (Input.GetMouseButtonUp(0))
        {
            if (arrastrandoSeleccion)
            {
                SeleccionarUnidadesDentroDelRecuadro();
            }
        }
    }

    /// <summary>
    /// Si hay unidades seleccionadas y hacemos clic derecho sobre el suelo,
    /// enviamos a todas al punto indicado.
    /// </summary>
    private void GestionarMovimientoDeSeleccionados()
    {
        // Solo actuamos si hay unidades seleccionadas.
        if (unidadesSeleccionadas.Count == 0) return;

        // Usamos clic derecho para mover, para no mezclarlo con el arrastre de selección.
        if (Input.GetMouseButtonDown(1))
        {
            Ray rayo = camaraPrincipal.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Lanzamos el rayo contra el suelo.
            if (Physics.Raycast(rayo, out hit, 500f, capaSuelo))
            {
                // Opcionalmente ajustamos el destino al punto más cercano válido del NavMesh.
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(hit.point, out navHit, 2f, NavMesh.AllAreas))
                {
                    // Mandamos todas las unidades seleccionadas al mismo punto.
                    foreach (UnidadSeleccionable unidad in unidadesSeleccionadas)
                    {
                        unidad.MoverA(navHit.position);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Deselecciona todas las unidades actualmente seleccionadas.
    /// </summary>
    private void DeseleccionarTodo()
    {
        foreach (UnidadSeleccionable unidad in unidadesSeleccionadas)
        {
            unidad.Deseleccionar();
        }

        unidadesSeleccionadas.Clear();
    }

    /// <summary>
    /// Selecciona todas las unidades cuya posición en pantalla cae dentro del rectángulo.
    /// </summary>
    private void SeleccionarUnidadesDentroDelRecuadro()
    {
        // Antes de seleccionar nuevas, limpiamos la selección anterior.
        DeseleccionarTodo();

        // Obtenemos el rectángulo actual de selección en coordenadas de pantalla.
        Rect rectSeleccion = ObtenerRectanguloPantalla(posicionInicialRaton, posicionActualRaton);

        // Recorremos todas las unidades de la escena.
        foreach (UnidadSeleccionable unidad in todasLasUnidades)
        {
            if (unidad == null) continue;

            // Convertimos la posición del mundo de la unidad a posición de pantalla.
            Vector3 posicionPantalla = camaraPrincipal.WorldToScreenPoint(unidad.transform.position);

            // Si la unidad está detrás de la cámara, no la seleccionamos.
            if (posicionPantalla.z < 0f) continue;

            // Ojo: WorldToScreenPoint usa el origen abajo a la izquierda,
            // así que aquí podemos trabajar directamente con esas coordenadas.
            Vector2 puntoPantalla = new Vector2(posicionPantalla.x, posicionPantalla.y);

            // Si el punto está dentro del rectángulo, seleccionamos la unidad.
            if (rectSeleccion.Contains(puntoPantalla))
            {
                unidad.Seleccionar();
                unidadesSeleccionadas.Add(unidad);
            }
        }
    }

    /// <summary>
    /// Devuelve un rectángulo en pantalla a partir de dos puntos:
    /// el inicio del arrastre y la posición actual/final del ratón.
    /// </summary>
    private Rect ObtenerRectanguloPantalla(Vector2 inicio, Vector2 fin)
    {
        // Calculamos la esquina inferior izquierda del rectángulo.
        float x = Mathf.Min(inicio.x, fin.x);
        float y = Mathf.Min(inicio.y, fin.y);

        // Calculamos ancho y alto como valor absoluto de la diferencia.
        float width = Mathf.Abs(inicio.x - fin.x);
        float height = Mathf.Abs(inicio.y - fin.y);

        return new Rect(x, y, width, height);
    }

    private void OnGUI()
    {
        // Solo dibujamos el rectángulo mientras estamos arrastrando.
        if (!arrastrandoSeleccion) return;

        Rect rect = ObtenerRectanguloPantalla(posicionInicialRaton, posicionActualRaton);

        // En OnGUI el origen está arriba a la izquierda, así que debemos convertir la Y.
        rect.y = Screen.height - rect.y - rect.height;

        // Dibujamos el relleno.
        Color colorAnterior = GUI.color;
        GUI.color = colorRelleno;
        GUI.DrawTexture(rect, texturaBlanca);

        // Dibujamos el borde superior.
        GUI.color = colorBorde;
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 2f), texturaBlanca);

        // Borde inferior.
        GUI.DrawTexture(new Rect(rect.x, rect.yMax - 2f, rect.width, 2f), texturaBlanca);

        // Borde izquierdo.
        GUI.DrawTexture(new Rect(rect.x, rect.y, 2f, rect.height), texturaBlanca);

        // Borde derecho.
        GUI.DrawTexture(new Rect(rect.xMax - 2f, rect.y, 2f, rect.height), texturaBlanca);

        // Restauramos el color original de GUI.
        GUI.color = colorAnterior;
    }
}