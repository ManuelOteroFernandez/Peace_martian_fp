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

    [Header("AI Movement")]
    [SerializeField] protected int patrolWaypointsRange = 2;
    List<Waypoint> patrolRoute;
    List<Waypoint> routeToOrigin;
    int patrolWaypointIndex = 0;
    int routeToOriginWaypointIndex = 0;
    protected Waypoint currentWaypoint;
    protected Waypoint nextWaypoint;
    protected Waypoint originWaypoint;

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
        patrolRoute = aiAgent.GetPatrolRoute(patrolWaypointsRange);
        originWaypoint = aiAgent.GetCurrentWaypoint();
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
        currentWaypoint = aiAgent.GetCurrentWaypoint();
        nextWaypoint = currentWaypoint.bestNextWaypoint;

        if (nextWaypoint == null){
            return;
        }

        Vector2 direction = nextWaypoint.position - currentWaypoint.position;
        Flip(-direction.x);
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        if (IsGrounded() || currentWaypoint.type != WaypointType.Cliff) {
            Vector2 horizontalVelocity = direction * chaseSpeed;
            rigidbody2D.linearVelocity = new Vector2(horizontalVelocity.x, rigidbody2D.linearVelocity.y);
        }

        if (Vector2.Distance(enemyPosition, nextWaypoint.position) < 0.5f){
            if (nextWaypoint.type == WaypointType.Ladder) {
                transform.position = new Vector3(nextWaypoint.position.x, transform.position.y, 0);
            }
            aiAgent.AdvanceToNextWaypoint();
        }
    }

    public void Patrol() {
        currentWaypoint = aiAgent.GetCurrentWaypoint();
        nextWaypoint = patrolRoute[patrolWaypointIndex];

        Vector2 direction = nextWaypoint.position - currentWaypoint.position;
        Flip(-direction.x);
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        Vector2 horizontalVelocity = direction.normalized * patrolSpeed;
        rigidbody2D.linearVelocity = new Vector2(horizontalVelocity.x, rigidbody2D.linearVelocity.y);

        if (Vector2.Distance(enemyPosition, nextWaypoint.position) < 0.5f){
            if (patrolWaypointIndex + 1 == patrolRoute.Count) {
                patrolWaypointIndex--;
            } else {
                patrolWaypointIndex++;
            }

            aiAgent.RelocateCurrentWaypoint();
        }
    }

    //TODO: Refactorizar
    public void ReturnToOrigin() {
        if (!IsBackInOrigin()) {
            //Se calcula la ruta al origen si esta no existe
            if (routeToOrigin == null) {
                routeToOrigin = aiAgent.FindRouteToWaypoint(originWaypoint);
            }

            //Evitamos un indexOutOfBoundsException
            if (routeToOriginWaypointIndex >= routeToOrigin.Count) {
                routeToOriginWaypointIndex = 0;
                return;
            }

            currentWaypoint = aiAgent.GetCurrentWaypoint();
            nextWaypoint = routeToOrigin[routeToOriginWaypointIndex];

            //Si el enemigo se encuentra con una subida que no sea escalera, se queda patrullando en el sitio en el que está
            if (nextWaypoint.type == WaypointType.Cliff && nextWaypoint.position.y > currentWaypoint.position.y){
                originWaypoint = currentWaypoint;
                patrolRoute = aiAgent.GetPatrolRoute(originWaypoint, patrolWaypointsRange);
                return;
            }

            enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

            Vector2 direction = nextWaypoint.position - currentWaypoint.position;

            //En caso de que la dirección sea cero, significa que el WP actual y el siguiente con el mismo, así que avanzamos en la lista.
            if (direction.Equals(Vector2.zero)) {
                routeToOriginWaypointIndex++;
                return;
            }

            Flip(-direction.x);
            rigidbody2D.linearVelocity = direction.normalized * patrolSpeed;

            //Cuando el enemigo llega al siguiente WP, se actualiza el currentWaypoint
            if (Vector2.Distance(enemyPosition, nextWaypoint.position) < 0.6f){
                aiAgent.RelocateCurrentWaypoint();
            }

            //Cuando el enemigo llega al originWaypoint, se reinicia la ruta al origen y se vuelve a patrullar
            if (currentWaypoint == originWaypoint) {
                routeToOrigin = null;
                routeToOriginWaypointIndex = 0;
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

    public bool IsBackInOrigin() {
        return Vector2.Distance(currentWaypoint.position, originWaypoint.position) < 0.5f;
    }

    public bool CantReachTarget() {
        return nextWaypoint == null && !IsTargetInChaseRange();
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

    public float DistanceToOriginWaypoint() {
        return Vector2.Distance(currentWaypoint.position, originWaypoint.position);
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

        if (currentWaypoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(originWaypoint.position, 0.3f); // Dibuja el originWaypoint
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(currentWaypoint.position, 0.3f); // Dibuja el currentWaypoint
            Gizmos.DrawLine(currentWaypoint.position, originWaypoint.position);

            if (nextWaypoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(nextWaypoint.position, 0.3f); // Dibuja el nextWaypoint
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(currentWaypoint.position, nextWaypoint.position); // Dibuja la conexión entre ellos
            }
        }
    }

    public abstract void EnemyAttack();
}
