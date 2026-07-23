using UnityEngine;
using UnityEngine.AI;

public class TroopsMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTroops(Vector3 target)
    {
        if (!agent.isActiveAndEnabled) return;
        if (!agent.isOnNavMesh) return;
        agent.SetDestination(target); //manda o navmesh ao ponto onde o raycast tocou o solo quando clicamos
    }
}
