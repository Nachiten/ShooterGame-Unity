using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public float distanteToFollow = 70f;
    private Transform player;
    
    // create navmesh agent
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("First Person Player").GetComponent<Transform>();
    }

    private void Update()
    {
        // if player is in range
        if (Vector3.Distance(transform.position, player.position) < distanteToFollow)
            // set destination to player
            agent.destination = player.position;
    }
}
