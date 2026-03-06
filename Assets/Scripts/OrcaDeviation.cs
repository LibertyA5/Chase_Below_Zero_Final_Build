using UnityEngine;
using UnityEngine.AI;

public class OrcaDeviation : MonoBehaviour
{
    public Transform seal;
    public float detectionRange = 20f;
    public float chaseSpeed = 10f;
    public float lingerTime = 4f;

    public LayerMask whatIsGround;

    SealMovement sealMovement;
    NavMeshAgent agent;
    Orca patrol;

    bool chasing;
    bool searching;
    float searchTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patrol = GetComponent<Orca>();
        sealMovement = seal.GetComponent<SealMovement>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, seal.position);

        if (!chasing && !searching)
        {
            if (distance <= detectionRange && !sealMovement.IsHidden)
            {
                chasing = true;
                patrol.enabled = false;
                agent.enabled = false;
            }
        }
        if (chasing)
        {
            if (sealMovement.IsHidden)
            {
                chasing = false;
                searching = true;
                searchTimer = lingerTime;
                return;
            }

            Vector3 dir = (seal.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, 2f, whatIsGround))
            {
                dir += hit.normal;
                dir.Normalize();
            }

            transform.position += dir * chaseSpeed * Time.deltaTime;

            if (dir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
            }
        }
        if (searching)
        {
            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0f)
            {
                searching = false;
                patrol.enabled = true;
                agent.enabled = true;
            }
        }
    }
}