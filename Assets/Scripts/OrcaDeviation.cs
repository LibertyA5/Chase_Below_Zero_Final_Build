using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcaDeviation : MonoBehaviour
{
    public Transform seal;
    public float chaseSpeed = 10f;
    public float lingerTime = 4f;

    public LayerMask whatIsGround;
    public LayerMask whatIsSeal;

    SealMovement sealMovement;
    NavMeshAgent agent;
    Orca patrol;

    public bool chasing;
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
        if (chasing)
        {
            if (sealMovement.IsHidden)
            {
                StopChase();
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
    void StartChase()
    {
        if (chasing) return;

        chasing = true;
        patrol.enabled = false;
        agent.enabled = false;
    }
    void StopChase()
    {
        chasing = false;
        searching = true;
        searchTimer = lingerTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsSeal) != 0)
        {
            if (!sealMovement.IsHidden)
            {
                StartChase();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsSeal) != 0)
        {
            if (chasing)
            {
                StopChase();
            }
        }
    }
}