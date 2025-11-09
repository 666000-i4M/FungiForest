using UnityEngine;
using UnityEngine.AI;

public class WormAI : MonoBehaviour
{
    public Transform player; // Referencia al transform del jugador (asigna en el Inspector)
    public float detectionRadius = 5f; // Radio para detectar al jugador y empezar a huir
    public float normalSpeed = 3f; // Velocidad normal de patrulla
    public float fleeSpeed = 7f; // Velocidad más rápida al huir
    public float patrolRadius = 20f; // Radio alrededor del NPC para generar puntos de patrulla aleatorios
    public float minDistanceToPoint = 1f; // Distancia mínima para considerar que llegó a un punto

    private NavMeshAgent agent;
    private Vector3 targetPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        PickNewPatrolPoint(); // Elige el primer punto aleatorio
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Jugador cerca: huye en dirección opuesta
            agent.speed = fleeSpeed;
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            Vector3 fleePosition = transform.position + fleeDirection * (detectionRadius * 2f); // Huye a una posición alejada

            // Asegura que la posición de huida esté en el NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleePosition, out hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            // No hay jugador cerca: patrulla normal
            agent.speed = normalSpeed;

            // Si llegó al punto actual o no tiene camino, elige uno nuevo
            if (!agent.pathPending && agent.remainingDistance <= minDistanceToPoint)
            {
                PickNewPatrolPoint();
            }
        }
    }

    void PickNewPatrolPoint()
    {
        // Genera un punto aleatorio dentro del radio de patrulla
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Mantiene en el plano del terreno (ajusta si es 3D completo)

        // Encuentra la posición válida más cercana en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            targetPoint = hit.position;
            agent.SetDestination(targetPoint);
        }
        else
        {
            // Si no encuentra, intenta de nuevo
            PickNewPatrolPoint();
        }
    }
}