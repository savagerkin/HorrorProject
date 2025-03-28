using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EmonsterController : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float stopRadius;
    [SerializeField] private float stopDistance;
    [SerializeField] private float displacementDistance;
    
    [SerializeField] private GameObject playerComponents;
    [SerializeField] private GameObject gameWorld;
    
    private Transform target;
    private NavMeshAgent agent;
    private bool canGoRight = false;
    private TreeChange treeChange;
    private bool canGoLeft = false;
    private bool timerStart = false;
    private float enemySeenTimer = 0;
    
    private FieldOfView playerFieldOfView;
    private FieldOfView enemyFieldOfView;

    private float _timer = 0;
    private float tickTimer = 0.2f;

    private ICanSeeTarget canSeeTarget;
    
    [Header("AI Debug Zone")]
    [SerializeField] private TextMeshProUGUI agentStateTextUI;
    [SerializeField] private bool canSeeTargetDebug;

    private void Start()
    {
        playerFieldOfView = playerComponents.GetComponentInChildren<FieldOfView>();
        enemyFieldOfView = GetComponentInChildren<FieldOfView>();
        canSeeTarget = playerComponents.GetComponentInChildren<ICanSeeTarget>();
        treeChange = gameWorld.GetComponent<TreeChange>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDistance;
        
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        float distance = Vector3.Distance(target.position, transform.position);
        canSeeTargetDebug = canSeeTarget.CanSeeTarget();
        
        if(timerStart)
            enemySeenTimer += Time.deltaTime;
        
        if (_timer > tickTimer)
        {
            _timer = 0;
            if (canSeeTarget.CanSeeTarget())
            {
                HandlePlayerSeen();
            }
            else
            {
                HandlePlayerNotSeen(distance);
            }
        }
        enemyFieldOfView.setCheckFOV(distance < 20f);
    }

    private bool TryMoveTo(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        
        return false;
    }

    private void MoveTowardsLeftOrRight()
    {
        Vector3 leftEndPoint = playerFieldOfView.getleftEndpoint().position;
        Vector3 rightEndPoint = playerFieldOfView.getrightEndpoint().position;

        if (TryMoveTo(leftEndPoint))
        {
            agent.SetDestination(leftEndPoint);
            agentStateTextUI.text = "Moving to left";
        }
        else if (TryMoveTo(rightEndPoint))
        {
            agent.SetDestination(rightEndPoint);
            agentStateTextUI.text = "Moving to right";
        }
        else
            Wander();
    }

    private void Wander()
    {
        Vector3 directionAwayFromPlayer = (transform.position - target.position).normalized;
        Vector3 wanderPosition = transform.position + directionAwayFromPlayer * 10f;
        
        if(NavMesh.SamplePosition(wanderPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            agentStateTextUI.text = "Wandering";
        }
        //we could make this disappear if the position is not valid
        else if(enemySeenTimer > 10f)
        {
            agent.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            agentStateTextUI.text = "Wandering Randomly";
        }
    }

    private void MoveToTree()
    {
        Vector3 nearestTree = treeChange.getNearestTreeMonster();
        Vector3 directionToPlayer = (target.position - nearestTree).normalized;
        Vector3 positionBehindTree = nearestTree - directionToPlayer * 1f;
        
        if(TryMoveTo(positionBehindTree))
        {
            agent.SetDestination(positionBehindTree);
            agentStateTextUI.text = "Moving to the tree";
        }
        else
        {
            MoveTowardsLeftOrRight();
        }
    }

    private void ChasePlayer()
    {
        if (TryMoveTo(target.position))
        {
            agent.SetDestination(target.position);
            agentStateTextUI.text = "Chasing Player";
        }
    }

    private void HandlePlayerSeen()
    {
        agent.speed = 10f;
        timerStart = true;
        MoveToTree();
    }

    private void HandlePlayerNotSeen(float distance)
    {
        enemySeenTimer = 0;
        timerStart = false;
        agent.speed = 2f;
        
        if (distance < stopRadius)
        {
            agentStateTextUI.text = "distance < stopRadius";
            Wander();
        }
        else
        {
            agentStateTextUI.text = "Chasing Player";
            ChasePlayer();
        }
    }
}
