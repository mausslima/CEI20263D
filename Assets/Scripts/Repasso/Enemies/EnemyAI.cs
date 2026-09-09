using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State actualState = State.Patrol;
    private Transform player;
    [SerializeField] float chaseRange = 7f;
    private float stopDistance = 2f;

    //---Patrol---//
    [SerializeField] float patrolRadius = 5f;
    [SerializeField] float waitingTime = 2f;
    private Vector3 originPoint;
    private float arrivalTime;
    private bool isWaiting;

    //---Attack---//
    private float damage = 10f;
    private float attackRate = 1.5f;
    private float nextAttack;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        agent.stoppingDistance = stopDistance;

        if (player != null) playerHealth = player.GetComponent<PlayerHealth>();

        originPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        switch (actualState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
        }
    }
    private void ChangeState(State newState)
    {
        actualState = newState;

        if (newState == State.Patrol)
        {
            agent.ResetPath();
            isWaiting = false;
        }
    }
    private void Patrol()
    {
        if (SeePlayer())
        {
            ChangeState(State.Chase); 
            return;
        }

        if (isWaiting)
        {
            if (Time.time >= arrivalTime + waitingTime)
            {
                isWaiting = false;
                GoToRandomPoint();
            }
            return;
        }

        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            arrivalTime = Time.time;
        }
    }
    private void Chase()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > chaseRange * 1.5f)
        {
            ChangeState (State.Patrol);
            return;
        }

        agent.SetDestination(player.position);

        if (distance > stopDistance + 0.5f)
        {
            ChangeState(State.Attack);
        }
    }
    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance +1f)
        {
            ChangeState(State.Chase);
            return;
        }

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);

        if (Time.time >= nextAttack)
        {
            if (playerHealth != null) playerHealth.TakeDamage(damage);
            nextAttack = Time.time + attackRate;
        }
    }
    private bool SeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > chaseRange) return false;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 target = player.position + Vector3.up * 0.5f;
        RaycastHit hit;
        if (Physics.Raycast(origin, target - origin, out hit, chaseRange))
        {
            return hit.transform == player || hit.transform.IsChildOf(player);
        }
        return false;
    }
    private void GoToRandomPoint()
    {
        Vector3 randomPoint = originPoint + Random.insideUnitSphere * patrolRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? originPoint : transform.position, patrolRadius);
    }
}
