using UnityEngine;
using UnityEngine.AI;

// Este script va en cada esfera/unidad.
// Se encarga de:
// 1. Guardar si la unidad está seleccionada.
// 2. Cambiar de color al seleccionarse.
// 3. Mover la unidad con NavMeshAgent cuando reciba una orden.
[RequireComponent(typeof(NavMeshAgent))]
public class UnidadSeleccionable : MonoBehaviour
{
    // Color normal de la unidad cuando NO está seleccionada.
    [Header("Colores")]
    [SerializeField] private Color colorNormal = Color.white;

    // Color que tendrá la unidad cuando SÍ esté seleccionada.
    [SerializeField] private Color colorSeleccionado = Color.green;

    // Referencia al Renderer para cambiar el material/color.
    private Renderer miRenderer;

    // Referencia al NavMeshAgent para mover la unidad.
    private NavMeshAgent agent;

    // Estado de selección de la unidad.
    public bool EstaSeleccionada;

    private void Awake()
    {
        // Guardamos referencias a los componentes.
        miRenderer = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Al comenzar, la unidad no está seleccionada.
        Deseleccionar();
    }

    /// <summary>
    /// Marca esta unidad como seleccionada y cambia su color.
    /// </summary>
    public void Seleccionar()
    {
        EstaSeleccionada = true;

        if (miRenderer != null)
        {
            miRenderer.material.color = colorSeleccionado;
        }
    }

    /// <summary>
    /// Marca esta unidad como no seleccionada y restaura su color normal.
    /// </summary>
    public void Deseleccionar()
    {
        EstaSeleccionada = false;

        if (miRenderer != null)
        {
            miRenderer.material.color = colorNormal;
        }
    }

    /// <summary>
    /// Envía la unidad a un punto del NavMesh.
    /// </summary>
    /// <param name="destino">Punto al que queremos mover la unidad.</param>
    public void MoverA(Vector3 destino)
    {
        // Comprobamos que el agente está activo y colocado sobre el NavMesh.
        if (!agent.isActiveAndEnabled) return;
        if (!agent.isOnNavMesh) return;

        // Le damos la orden de movimiento.
        agent.SetDestination(destino);
    }
}