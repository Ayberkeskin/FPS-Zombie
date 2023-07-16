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
    [SerializeField] int damage= 2;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //daire ciz
        Gizmos.DrawSphere(transform.position,chaseRange);

        switch (curentState)
        {
            case State.Search:
                Gizmos.color = Color.blue;
                Vector3  targetPos=new Vector3(agent.destination.x,transform.position.y,agent.destination.z);
                //cizgi ciz
                Gizmos.DrawLine(transform.position, targetPos);
                break;
            case State.Chase:
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine (transform.position,player.position);
                break;
            case State.Attack:
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, player.position);
                break;
        }
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
                break;
            case State.Search:
                //agent.remainingDistance=hedef noktasý ile kendi arasýndaki kalan mesafeyi döndürür
                if (!isSearched&&agent.remainingDistance<=0.1f||!agent.hasPath&&!isSearched)//agent.hasPath gidecek bir yer var yada yok döndürür
                {
                    //Navmesh açýk iken transformu deðiþtiremezsiniz navmeshi kapatman lazým
                    Vector3 agentTarget = new Vector3(agent.destination.x,transform.position.y,agent.destination.z);
                    // Navmesh agentý kapatma
                    agent.enabled = false;

                    transform.position = agentTarget;//pozisyonu deðiþtik normalde navmesh buna izin vermez ondan kapatýk

                    //Navmesh agentý geri açma üst kýsým kapalý olduðunda olacak þeylerde Navmesh kapalý olacak Navmeshin izin vermediði þeyler olacak
                    agent.enabled=true;
                    //Invoke belli bir süre sonra fonksiyonu çalýþtýrýr
                    Invoke("Search",patrolWaitTime);
                    isSearched = true;
                }  
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
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
        agent.velocity = Vector3.zero;
        agent.isStopped = true; // enemyinin hýzýný sýfýra çeker durdurur ama hemen deðil yavasca ondan dolayý üste hýzý sýfýrladým
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
        //hedefe doðru hareket
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
        // Random.insideUnitSpher bir dairede random bir konum seçer 1 radiusluk ondan dolayý patrolRadius ile çarptýk
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius; 
        randomDirection += transform.position;
        NavMeshHit hit;// raycast hit gibi
        //Navmesh üzerinde gidilebilir olup olmadýðýný kontrol eder eðer gidilemez ise en yakýn gidilebilir konumu verir
        NavMesh.SamplePosition(randomDirection,out hit ,patrolRadius,1);//sondaki 1 navmeshde katmanlar var onu temsil ediyor sadece 1 tane var bizde
        // 1 yerine Navmesh.AllAreas yazarsan tüm bölgelerde çalýþ demek
        return hit.position;
    }

    public int GetDamage()
    {  
        return damage;
    }
}
