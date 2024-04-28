using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public Vector3 minPosition = new Vector3(0f, 0f, 0f);
    public Vector3 maxPosition = new Vector3(0f, 0f, 0f);
    public float baseMovementInterval = 60f; 
    public float variance = 10f; 
    public bool snapToNavmesh = true;
    private float timer;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        timer = baseMovementInterval;

        navMeshAgent = GetComponent<NavMeshAgent>();

        MoveToRandomPosition();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            MoveToRandomPosition();
            timer = baseMovementInterval + Random.Range(-variance, variance);
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x),
                                             Random.Range(minPosition.y, maxPosition.y),
                                             Random.Range(minPosition.z, maxPosition.z));

		if (snapToNavmesh = true)
		{
			NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 100, -1);

			if (navMeshAgent != null)
			{
				navMeshAgent.SetDestination(hit.position);
			}
			else
			{
				transform.position = hit.position;
			}
		}
	}
}