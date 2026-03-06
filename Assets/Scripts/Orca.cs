using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orca : MonoBehaviour
{
    [SerializeField] float waitTimeOnWayPoint = 1f;
    [SerializeField] Path path;

    NavMeshAgent agent;

    float time = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        agent.destination = path.GetCurrentWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= 0.1f)
        {
            time += Time.deltaTime;
            if (time >= waitTimeOnWayPoint)
            {
                time = 0f;
                agent.destination = path.GetNextWayPoint();
            }
        }
        // float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
    }
}
