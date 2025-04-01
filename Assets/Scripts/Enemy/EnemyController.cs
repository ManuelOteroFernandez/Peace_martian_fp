using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Rigidbody2D rigidbody2D {get; private set;}
    [SerializeField] public LayerMask obstaclesLayerMask;
    protected SpriteRenderer spriteRenderer;

    [Header("Movement Parameters")]
    [SerializeField] protected float chaseSpeed = 5f;
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected float chaseDistance = 10f;
    [SerializeField] protected float maxDistanceFromOrigin = 5f;
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

    public float AttackDuration => attackDuration;
    public float MaxDistanceFromOrigin => maxDistanceFromOrigin;

    protected virtual void Awake(){
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAgent = GetComponent<AIAgent>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Start(){
        aiAgent.GetPatrolRoute();
        ChangeState(new EnemyPatrolState());
    }

    protected virtual void FixedUpdate(){
        UpdateCooldowns();
        UpdateState();
    }

    void UpdateState() {
        currentState.Update(this);
    }

    public void ChangeState(EnemyState newState) {
        currentState?.Exit(this);
        currentState = newState;

        Debug.Log("current state: " + currentState);

        currentState.Enter(this);
    }

    //TODO: Necesita ajuste
    public virtual void MoveEnemy(){
        aiAgent.UpdateFieldFlowNextWaypoint();

        if (aiAgent.nextWaypoint == null){
            return;
        }

        Vector2 direction = aiAgent.DirectionToNextWaypoint();
        Flip(-direction.x);
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        if (IsGrounded() || aiAgent.currentWaypoint.type != WaypointType.Cliff) {
            Vector2 horizontalVelocity = direction * chaseSpeed;
            rigidbody2D.linearVelocity = new Vector2(horizontalVelocity.x, rigidbody2D.linearVelocity.y);
        }

        if (Vector2.Distance(enemyPosition, aiAgent.nextWaypoint.position) < 0.5f){
            if (aiAgent.nextWaypoint.type == WaypointType.Ladder) {
                transform.position = new Vector3(aiAgent.nextWaypoint.position.x, transform.position.y, 0);
            }
            aiAgent.AdvanceToNextWaypoint();
        }
    }

    public void Patrol() {
        aiAgent.UpdateNextPatrolWaypoint();

        Vector2 direction = aiAgent.DirectionToNextWaypoint();
        Flip(-direction.x);
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        Vector2 horizontalVelocity = direction.normalized * patrolSpeed;
        rigidbody2D.linearVelocity = new Vector2(horizontalVelocity.x, rigidbody2D.linearVelocity.y);

        if (Vector2.Distance(enemyPosition, aiAgent.nextWaypoint.position) < 0.5f){
            aiAgent.UpdatePatrolWaypointIndex();
            aiAgent.RelocateCurrentWaypoint();
        }
    }

    //TODO: Refactorizar
    public void ReturnToOrigin() {
        if (!aiAgent.IsInOriginWaypoint()) {
            //Se calcula la ruta al origen si esta no existe
            aiAgent.CalculateRouteToOrigin();

            //Evitamos un indexOutOfBoundsException
            if (aiAgent.RouteIndexOutOfBounds()) {
                aiAgent.ResetRouteToOriginWaypointIndex();
                return;
            }

            aiAgent.UpdateRouteToOriginNextWaypoint();

            //Si el enemigo se encuentra con una subida que no sea escalera, se queda patrullando en el sitio en el que está
            if (aiAgent.nextWaypoint.type == WaypointType.Cliff && aiAgent.nextWaypoint.position.y > aiAgent.currentWaypoint.position.y){
                aiAgent.UpdateOriginWaypoint(aiAgent.currentWaypoint);
                aiAgent.GetPatrolRoute();
                return;
            }

            enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

            Vector2 direction = aiAgent.nextWaypoint.position - aiAgent.currentWaypoint.position;

            //En caso de que la dirección sea cero, significa que el WP actual y el siguiente con el mismo, así que avanzamos en la lista.
            if (direction.Equals(Vector2.zero)) {
                aiAgent.IncreaseRouteToOriginWaypointIndex();
                return;
            }

            Flip(-direction.x);
            rigidbody2D.linearVelocity = direction.normalized * patrolSpeed;

            //Cuando el enemigo llega al siguiente WP, se actualiza el currentWaypoint
            if (Vector2.Distance(enemyPosition, aiAgent.nextWaypoint.position) < 0.6f){
                aiAgent.RelocateCurrentWaypoint();
            }

            //Cuando el enemigo llega al originWaypoint, se reinicia la ruta al origen y se vuelve a patrullar
            if (aiAgent.IsInOriginWaypoint()) {
                aiAgent.ResetRouteToOrigin();
            }
        }
    }

    public void Flip(float direction) {
        if (direction != 0) {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }
    }

    public bool IsGrounded() {
        return !enemyHealth.IsInBubble() && rigidbody2D.linearVelocityY == 0;
    }

    public bool IsTargetInChaseRange() {
        return Vector3.Distance(target.position, transform.position) < chaseDistance;
    }

    public bool IsTargetInAttackRange() {
        return IsTargetInAttackRange(target);
    }

    public bool CantReachTarget() {
        return aiAgent.nextWaypoint == null && !IsTargetInChaseRange();
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
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        if (target != null) {
            Vector2 direction = (target.position - transform.position).normalized;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * attackRange));
        }
    }

    public abstract void EnemyAttack();
}
