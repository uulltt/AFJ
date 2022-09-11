using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class CharacterLocomotor : MonoBehaviour
{
    private Vector3 movementTarget;
    private bool moving;
    private NavMeshAgent agent;

    private float DeadZone = 0.05f;

    public float Speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    public void MoveTowardsSpot(Vector3 target)
    {
        movementTarget = target;
        moving = true;
        agent.SetDestination(target);
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, movementTarget) <= DeadZone)
        {
            agent.isStopped = true;
            moving = false;
        }


        GetComponentInParent<Animator>().SetFloat("Speed", GetComponent<NavMeshAgent>().isStopped ? 0 : 1);
    }
}
