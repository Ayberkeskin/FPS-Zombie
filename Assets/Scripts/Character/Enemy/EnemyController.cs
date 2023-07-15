using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Idle,
        Search,
        Chase,
        Attack
    }

    private NavMeshAgent agent;

    private Transform player;

    [SerializeField] float attackRange = 2f;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 15f;
    [SerializeField] float patrolRadius = 6f;
    [SerializeField] float patrolWaitTime = 2f;
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float searchSpeed = 3.5f;

    private bool isSearched=false;


    [SerializeField] private State curentState=State.Idle;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        StateCheck();
        StateExecute();
    }

    private void StateCheck()
    {
        float distanceToTarget=Vector3.Distance(player.position,transform.position);

        if (distanceToTarget<=chaseRange&&distanceToTarget>attackRange)
        {
            curentState = State.Chase;
        }
        else if (distanceToTarget<=attackRange)
        {
            curentState=State.Attack;
        }
        else
        {
            curentState = State.Search;
        }
    }

    private void StateExecute()
    {
        switch (curentState)
        {
            case State.Idle:
                print("Idle");
                break;
            case State.Search:
                print("Search");
                if (!isSearched&&agent.remainingDistance<=0.1f||!agent.hasPath&&!isSearched)
                {
                    Vector3 agentTarget = new Vector3(agent.destination.x,transform.position.y,agent.destination.z);
                    agent.enabled = false;
                    transform.position = agentTarget;
                    agent.enabled=true;

                    Invoke("Search",patrolWaitTime);
                    isSearched = true;
                }  
                break;
            case State.Chase:
                print("Chase");
                Chase();
                break;
            case State.Attack:
                Attack();
                print("Attack");
                break;
        }
    }

    private void Search()
    {
        agent.isStopped = false;
        agent.speed = searchSpeed;
        isSearched = false;
        agent.SetDestination(GetRandomPosition());
    }
    private void Attack()
    {
        if (player==null)
        {
            return;
        }
        agent.isStopped = true; // enemyinin hýzýný sýfýra çeker durdurur
        LookTheTargert(player.position);
    }

    private void Chase()
    {
        if (player==null)
        {
            return;
        }
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }
    private void LookTheTargert(Vector3 target)
    {
        Vector3 lookPos = new Vector3(target.x,transform.position.y,target.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos - transform.position),
            turnSpeed * Time.deltaTime); 
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection,out hit ,patrolRadius,1);
        return hit.position;
    }
}
