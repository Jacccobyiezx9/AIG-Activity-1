using UnityEngine;
using UnityEngine.AI;

public class ChaseJump : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 5f;

    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent m_Agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Agent.SetDestination(player.position);

        if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
        {
            anim.SetBool("Running", false);
        }
        else
        {
            anim.SetBool("Running", true);
        }
    }
}
