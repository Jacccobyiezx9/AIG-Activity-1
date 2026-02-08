using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private int currentPoint = 0;
    [SerializeField] private bool pointReached = false;

    [Header("Chase")]
    [SerializeField] private GameObject detectIcon;
    [SerializeField] private float fieldOfView = 45f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float detectionTime = 5f;
    [SerializeField] private bool chasing = false;
    [SerializeField] private Vector3 lastPlayerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        detectIcon.SetActive(false);
        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[currentPoint].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!chasing)
        {
            detectIcon.SetActive(false);
            agent.speed = 4f;
            Patrol();
            DetectPlayer();
        }
        else
        {
            detectIcon.SetActive(true);
            detectIcon.transform.LookAt(detectIcon.transform.position + cam.transform.forward);
            agent.speed = 6f;
            Chase();
        }

    }

    private void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            pointReached = true;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
            Debug.Log($"{name} has stopped because no valid path is available to the target.");
            return;
        }

        if (pointReached)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPoint].position;
            pointReached = false;
        }
    }


    private void Chase()
    {
        if (player == null) return;

        lastPlayerPos = player.position;
        Vector3 reachableTarget = GetReachablePosition(lastPlayerPos);

        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
            agent.destination = reachableTarget;
        }
        else
        {
            agent.isStopped = true;
            Debug.Log($"{name} has stopped because no valid path is available to the target.");
        }

        if (Vector3.Distance(transform.position, player.position) > detectionRadius * 1.5f)
        {
            chasing = false;
        }
    }


    private void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Distance
        if (distanceToPlayer <= detectionRadius)
        {
            // Field of View 
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer <= fieldOfView / 2f)
            {
                // Line of Sight
                if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, directionToPlayer.normalized, distanceToPlayer))
                {
                    chasing = true;
                }
            }
        }
    }
    private Vector3 GetReachablePosition(Vector3 target)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 1f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        //Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        //Field of View
        Gizmos.color = Color.blue;
        Vector3 forward = transform.forward * detectionRadius;
        float halfFOV = fieldOfView / 2f;

        Vector3 leftBoundary = Quaternion.Euler(0, -halfFOV, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, halfFOV, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
