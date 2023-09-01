using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleAI : MonoBehaviour {
    [SerializeField] Pool enemyPool;
    
    [Space(5)]
    [Header("Agent Field of View Properties")]
    [SerializeField] float viewRadius;
    [SerializeField] float viewAngle;

    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstacleMask;

    [Space(5)]
    [Header("Agent Properties")]
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float patrolRadius;

    private GameObject[] players;

    private Transform playerTarget;

    private Vector3 currentDestination;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private bool playerSeen;
    private int maxNumberOfNewDestinationBeforeDeath;
    private enum State {Wandering, Chasing};
    private State currentState;

    // Use this for initialization
    void Start () {
        currentDestination = RandomNavSphere(transform.position, patrolRadius, -1);
        maxNumberOfNewDestinationBeforeDeath = Random.Range(5, 50);
        players = GameObject.FindGameObjectsWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void CheckState()
    {
        FindVisibleTargets();

        switch(currentState)
        {
            case State.Chasing:
                ChaseBehavior();
                break;

            default:
                WanderBehavior();
                break;
        }
    }

    void WanderBehavior()
    {
        animator.SetTrigger("walk");
        navMeshAgent.speed = walkSpeed;

        float dist = navMeshAgent.remainingDistance;

        if (dist != Mathf.Infinity && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            currentDestination = RandomNavSphere(transform.position, patrolRadius, -1);
            navMeshAgent.SetDestination(currentDestination);
            maxNumberOfNewDestinationBeforeDeath--;
            if (maxNumberOfNewDestinationBeforeDeath <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void ChaseBehavior()
    {
        if (playerTarget != null)
        {
            animator.SetTrigger("run");
            navMeshAgent.speed = runSpeed;
            currentDestination = playerTarget.transform.position;
            navMeshAgent.SetDestination(currentDestination);
        }
        else
        {
            playerSeen = false;
            currentState = State.Wandering;
        }
    }

    #region Vision
    void FindVisibleTargets()
    {

        playerTarget = null;
        playerSeen = false;
        
        if (players.Length == 0)
        {
            return;
        }
        
        for (int i=0; i<players.Length; i++)
        {
            float dstToTarget = Vector3.Distance(transform.position, players[i].transform.position);
            Vector3 dirToTarget = (players[i].transform.position - transform.position).normalized;
            if(dstToTarget <= viewRadius && Vector3.Angle(transform.forward, dirToTarget) <= viewAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToTarget, out hit))
                {
                    if (hit.collider.tag == "Player")
                    {
                        playerSeen = true;
                        playerTarget = hit.transform;
                    }
                }
            }

            /*RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit))
            {
                //float dstToTarget = Vector3.Distance(transform.position, players[i].transform.position);
                if (dstToTarget <= viewRadius)
                {
                    if (Vector3.Angle(transform.forward, dirToTarget) <= viewAngle / 2)
                    {
                        if (hit.collider.tag == "Player")
                        {
                            playerSeen = true;
                            playerTarget = hit.transform;
                        }
                    }
                }
            }*/
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    #endregion

    private bool HasFoundPlayer()
    {
        bool result = false;
        
        if (players.Length == 0)
        {
            return result;
        }
        
        for(int i=0; i<players.Length; i++)
        {
            if (Vector3.Distance(players[i].transform.position, transform.position) <= navMeshAgent.radius*2)
            {
                result = true;
            }
        }

        return result;
    }
    
    void Update () {
        CheckState();

        if (playerSeen)
        {
            currentState = State.Chasing;
        } else
        {
            currentState = State.Wandering;
        }

        if (HasFoundPlayer())
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
	}
}
