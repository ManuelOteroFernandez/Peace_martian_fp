using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class EnemyController : MonoBehaviour
{
    //TODO: Si el enemigo está persiguiendo al jugador, cambia a otro estado (por ejemplo Attack) y al salir de ese estado el jugador
    //está fuera de su alcance, el enemigo se mantiene en Idle. Revisar este comportamiento.
    [Header("States")]
    EnemyState currentState;

    [Header("Components")]
    public Animator animator {get; private set;}
    public EnemyHealth enemyHealth {get; private set;}
    public AIAgent aiAgent {get; private set;}
    public Rigidbody2D rb2d {get; private set;}
    [SerializeField] public LayerMask obstaclesLayerMask;
    protected SpriteRenderer spriteRenderer;

    [Header("Movement Parameters")]
    [SerializeField] protected float chaseSpeed = 5f;
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected float chaseDistance = 20f;
    [SerializeField] protected float detectionDistance = 10f;
    [SerializeField] protected float maxDistanceFromOrigin = 5f;
    [SerializeField] protected float enemyCenterOffset = 0.5f;

    [SerializeField] protected float maxJumpFromCliff = 1;
    protected Vector2 enemyPosition;

    [Header("General Attack Parameters")]
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float attackCooldown = 2f;
    [SerializeField] protected float attackDuration = 1f;
    [SerializeField] protected float freeEnemyChance = 0.5f;
    protected float currentAttackCooldown;

    [Header("Target")]
    protected Transform target;
    protected Transform trappedEnemyTarget;

    [Header("Ground Check Properties")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Transform groundCheckPoint;

    public float AttackDuration => attackDuration;
    public float MaxDistanceFromOrigin => maxDistanceFromOrigin;

    protected virtual void Awake(){
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAgent = GetComponent<AIAgent>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Start(){
        aiAgent.Initialize();
        if (patrolSpeed == 0f) {
            ChangeState(new EnemyIdleState());
        } else {
            ChangeState(new EnemyPatrolState());
        }
    }

    protected virtual void Update()
    {
        animator.SetFloat("velocityX", Mathf.Abs(rb2d.linearVelocityX));
    }

    protected virtual void FixedUpdate()
    {
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y + enemyCenterOffset);
        UpdateCooldowns();
        UpdateState();
    }

    void UpdateState() {
        currentState.Update(this);
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;

        // Debug.Log("current state: " + currentState);
        aiAgent.RelocateCurrentWaypoint();
        aiAgent.nextWaypoint = null;

        currentState.Enter(this);
    }

    public virtual void MoveTo(Waypoint waypoint, float speed)
    {
        if (waypoint.type == WaypointType.Cliff && !CanJumpFromCliff(waypoint))
        {
            // Debug.Log("Cliff?");
            rb2d.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (waypoint.position - enemyPosition).normalized;
        // Debug.Log("Direccion normalizada : " + dir);
        Flip(-dir.x);
        
        if (!CanMoveInDirection(dir)) return;

        if (waypoint.type == WaypointType.Ladder)
        {
            rb2d.gravityScale = 0f;
        }
        else
        {
            rb2d.gravityScale = 1f;
            if (!IsGrounded())
            {
                dir.y = 0;
            }
        }

        rb2d.linearVelocity = dir * speed * Time.deltaTime;
    }

    protected bool CanMoveInDirection(Vector2 direction)
    {
        
        Vector2 dirOrigin = (aiAgent.originWaypoint.position - enemyPosition).normalized;

        return DistanceToOriginWaypoint() <= MaxDistanceFromOrigin || (direction.x > 0 == dirOrigin.x > 0);
    }

    protected bool CanJumpFromCliff(Waypoint waypoint)
    {
        return aiAgent.NumWaypointToGround(waypoint) <= maxJumpFromCliff;
    }

    protected bool IsArrivedToWaypoint(Waypoint waypoint)
    {
        float distance = Vector2.Distance(waypoint.position, enemyPosition);
        return distance <= 0.1f;
    }

    public void MoveToTarget()
    {
        aiAgent.UpdateFieldFlowNextWaypoint();
        if (aiAgent.nextWaypoint == null)
        {
            // Debug.Log("No Next waypoint");
            return;
        }
        if (aiAgent.nextWaypoint.bestNextWaypoint == aiAgent.currentWaypoint)
        {
            // Debug.Log("Next waypoint loop");
            return;
        }
        if (CantReachTarget())
        {
            // Debug.Log("No reach target");
            return;
        }
        if (IsInvalidWaypoint(aiAgent.nextWaypoint))
        {
            // Debug.Log("Invalid Waypoint");
            rb2d.linearVelocity = new Vector2(0.0f, rb2d.linearVelocityY);
            return;
        }

        MoveTo(aiAgent.nextWaypoint,chaseSpeed);

        if (IsArrivedToWaypoint(aiAgent.nextWaypoint))
        {
            if (aiAgent.nextWaypoint.type == WaypointType.Ladder)
            {
                transform.position = new Vector3(aiAgent.nextWaypoint.position.x, transform.position.y, 0);
            }

            aiAgent.AdvanceToNextWaypoint();
        }

    }

    public void Patrol() {
        aiAgent.UpdateNextPatrolWaypoint();

        MoveTo(aiAgent.nextWaypoint,patrolSpeed);

        if (IsArrivedToWaypoint(aiAgent.nextWaypoint)){
            aiAgent.UpdatePatrolWaypointIndex();
            aiAgent.RelocateCurrentWaypoint();
        }
    }

    protected bool IsInvalidWaypoint(Waypoint waypoint)
    {
        return waypoint.type == WaypointType.Cliff && waypoint.position.y > enemyPosition.y;
    }

    public void ReturnToOrigin() {
        if (aiAgent.nextWaypoint == null)
        {
            aiAgent.RelocateCurrentWaypoint();
            aiAgent.ResetRouteToOrigin();
            aiAgent.ResetRouteToOriginWaypointIndex();
            aiAgent.CalculateRouteToOrigin();
            aiAgent.UpdateRouteToOriginNextWaypoint();
            return;
        }

        if (IsInvalidWaypoint(aiAgent.nextWaypoint))
        {
            // Debug.Log("is invalid waypoint");
            aiAgent.UpdateOriginWaypoint(aiAgent.currentWaypoint);
            aiAgent.GetPatrolRoute();
            return;
        }

        MoveTo(aiAgent.nextWaypoint, patrolSpeed);

        if (IsArrivedToWaypoint(aiAgent.nextWaypoint)) {
            aiAgent.IncreaseRouteToOriginWaypointIndex();
            aiAgent.UpdateRouteToOriginNextWaypoint();
        }
    }

    public void Flip(float direction) {
        if (direction != 0) {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }
    }

    public bool IsGrounded() {
        return !enemyHealth.IsInBubble() && Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
    }

    public bool IsFalling() {
        return aiAgent.currentWaypoint.type == WaypointType.Cliff && !IsGrounded();
    }

    public bool IsTargerInDetectionRange()
    {
        return Vector3.Distance(target.position, transform.position) < detectionDistance;
    }

    public bool IsTargetInChaseRange()
    {
        return Vector3.Distance(target.position, transform.position) < chaseDistance;
    }

    public bool IsTargetInAttackRange() {
        return IsTargetInAttackRange(target);
    }

    public bool CantReachTarget() {
        return !IsTargetInChaseRange();
    }

    private bool IsTargetInAttackRange(Transform target) {
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackRange) {
            return false;
        }

        return Physics2D.Raycast(transform.position, direction, distance, obstaclesLayerMask).collider == null;
    }

    public bool IsTrappedEnemyInAttackRange() {
        GameObject[] trappedEnemies = GameObject.FindGameObjectsWithTag("BigBubbleTrap");

        foreach (GameObject trappedEnemy in trappedEnemies) {
            if (IsTargetInAttackRange(trappedEnemy.transform)) {
                trappedEnemyTarget = trappedEnemy.transform;

                return true;
            }
        }

        trappedEnemyTarget = null;
        return false;
    }

    public bool IsBackInOrigin() {
        return aiAgent.IsInOriginWaypoint();
    }

    public float DistanceToOriginWaypoint() {
        return aiAgent.DistanceToOriginWaypoint();
    }

    public bool ChooseToFreeEnemy() {
        return Random.Range(0f, 1f) < freeEnemyChance;
    }

    void UpdateCooldowns() {
        currentAttackCooldown -= Time.fixedDeltaTime;
    }

    public bool IsAttackCooldownReady() {
        return currentAttackCooldown <= 0;
    }

    public void StartChasing() {
        aiAgent.RelocateCurrentWaypoint();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemyPosition, 0.1f);
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        if (target != null) {
            Vector2 direction = (target.position - transform.position).normalized;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * attackRange));
        }

        //Dibuja el área de detección del suelo para depuración
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }

    public abstract void EnemyAttack();
}
