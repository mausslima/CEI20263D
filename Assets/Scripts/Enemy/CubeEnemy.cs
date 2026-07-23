using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CubeEnemy : MonoBehaviour
{
    private Transform player; //referencia do player que o inimigo vai perseguir

    private float chaseRange = 1500f; //distancia que o inimigo detectara o player

    private float stopDistance = 2f; //distancia minima que o inimigo ficara do player. evita en entre no player

    private NavMeshAgent agent; // componente navmesh do inimigo que controla o movimento

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; //busca o player na cena

        if (player == null) // se o player for nulo, tenta de novo
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)  //se player nao for nulo, atribui o transform dele
                player = playerObject.transform;
        }

        agent.stoppingDistance = stopDistance; //configura a distancia de parada 
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return; //para se o o jogador for nulo
        if (!agent.isActiveAndEnabled) return; //para se o navmesh nao esta ativado
        if (!agent.isOnNavMesh) return; //para se o agente nao tem um navmesh

        float distance = Vector3.Distance(transform.position, player.position); //calcula a distancia entre o inimigo e o player

        if (distance <= chaseRange)// se o jogador esta dentro da area de perseguicao
        {
            agent.SetDestination(player.position); //move o inimigo até ele
        }
        else
        {
            agent.ResetPath(); //senao, para
        }
    }
}
