using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class SphereEnemy : MonoBehaviour
{
    //Move o inimigo de acordo com o click do mouse na camera
    [SerializeField] private Camera mainCamera; //referencia a camera principal
    [SerializeField] private LayerMask groundLayer; //layer do solo para que o raycast detecte apenas o solo
    [SerializeField] private float rayDistance = 200f; //distancia maxima do raycast

    private NavMeshAgent agent; //referencia ao navmesh do inimigo

    private float referenceDistance; // distancia de referencia em que consideramos o tamanho correto
    private Vector3 referenceScale; // escala original do objeto

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GameObject.FindAnyObjectByType<Camera>();

        if (mainCamera == null ) //se nenhuma camera foi assignada, usa a principal
        {
            mainCamera = Camera.main;
        }

        referenceScale = transform.localScale; //guarda a escala inicial do inimigo

        referenceDistance = Vector3.Distance(mainCamera.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null) return; //se nenuma camera é encontrada nao da pra fazer o raycast

        // se o agente nao esta ativo e nao estao no navmesh, sai
        if (!agent.isActiveAndEnabled) return;
        if (!agent.isOnNavMesh) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); //cria um raio da camera até o mouse
            RaycastHit hit; //variavel onde guardamos a informacao de impacto

            if (Physics.Raycast(ray, out hit, rayDistance, groundLayer)) // lanca o raycast ate o layer do solo
            {
                agent.SetDestination(hit.point); //manda o navmesh ao ponto onde o raycast tocou o solo quando clicamos
            }
        }
        MantenerTamanoAparente();

    }
    private void MantenerTamanoAparente() //escala o objeto em funcao da distancia da camera para mantes o mesmo tamanho aparente na tela
    {
        if (mainCamera == null) return;

        float currentDistance = Vector3.Distance(mainCamera.transform.position, transform.position); //calculo da distancia entre camera e inimigo

        if (referenceDistance <= 0.001f) return; // evita divisoes estranhas e escalas invalidas

        float scaleFactor = currentDistance / referenceDistance; //calcula o fator de escala: se estiver mais longe, cresce, se mais perto, diminui

        transform.localScale = referenceScale * scaleFactor; //aplica a nova escala a original

    }
}
