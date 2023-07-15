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
        agent.SetDestination(player.position);
    }
    private void LookTheTargert(Vector3 target)
    {
        Vector3 lookPos = new Vector3(target.x,transform.position.y,target.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos - transform.position),
            turnSpeed * Time.deltaTime); 
    }
}
