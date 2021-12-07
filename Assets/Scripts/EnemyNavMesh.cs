using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public float distanteToFollow = 10f;
    private GameObject player;
    
    // create navmesh agent
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("First Person Player");
    }

    private void Update()
    {
        // if player is in range
        if (Vector3.Distance(transform.position, player.transform.position) < distanteToFollow)
        {
            // set destination to player
            agent.SetDestination(player.transform.position);
        }
    }
}
