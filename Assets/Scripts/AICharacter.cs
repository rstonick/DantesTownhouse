using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    public enum AIState { RoamingSearching, Chasing }
    private AIState currentState = AIState.RoamingSearching;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private Vector3 playerStartingPosition;
    private Vector3 roamingTarget;

    private float chaseTimer = 0f;
    private const float MAX_CHASE_TIME = 10f;
    private const float ROAMING_RADIUS = 10f;
    private const float detectionRange = 15f;
    private const float lostPlayerRange = 20f;

    private Animator animator;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerStartingPosition = playerTransform.position;
        PickNewRoamingTarget();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.RoamingSearching:
                RoamingSearchingLogic();
                break;

            case AIState.Chasing:
                ChasingLogic();
                break;
        }
       animator.SetFloat("vely", agent.velocity.magnitude / agent.speed);

    }

    void RoamingSearchingLogic()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            PickNewRoamingTarget();
            print("Picking new target");
        }

        // logic to switch to Chasing state if the player is detected
        if (Vector3.Distance(transform.position, playerTransform.position) < detectionRange)
        {
             SwitchState(AIState.Chasing);
        }
    }

    void ChasingLogic()
    {
        agent.SetDestination(playerTransform.position);
        print("Chasing player");
        chaseTimer += Time.deltaTime;
        if (chaseTimer >= MAX_CHASE_TIME)
        {
            SwitchState(AIState.RoamingSearching);
        }

         
         if (Vector3.Distance(transform.position, playerTransform.position) > lostPlayerRange)
         {
            SwitchState(AIState.RoamingSearching);
         }
    }

    void SwitchState(AIState newState)
    {
        currentState = newState;
        chaseTimer = 0f;

        if (newState == AIState.RoamingSearching)
        {
            PickNewRoamingTarget();
        }
    }

    void PickNewRoamingTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * ROAMING_RADIUS;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, ROAMING_RADIUS, 1);
        roamingTarget = hit.position;
        agent.SetDestination(roamingTarget);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform.position = playerStartingPosition;
            SwitchState(AIState.RoamingSearching);
        }
    }
}
