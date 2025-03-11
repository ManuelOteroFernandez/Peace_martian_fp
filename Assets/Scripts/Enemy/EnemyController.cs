using System.Collections;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
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
    protected Vector2 enemyPosition;

    [Header("General Attack Parameters")]
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float attackCooldown = 2f;
    [SerializeField] protected float attackDuration = 1f;
    protected float currentAttackCooldown;

    [Header("AI Movement")]
    Waypoint currentWaypoint;
    Waypoint nextWaypoint;

    [Header("Player")]
    protected Transform target;

    public float AttackDuration => attackDuration;

    protected virtual void Awake(){
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAgent = GetComponent<AIAgent>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Start(){
        ChangeState(new EnemyIdleState());
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
    public void MoveEnemy(){
        currentWaypoint = aiAgent.GetCurrentWaypoint();
        nextWaypoint = currentWaypoint.bestNextWaypoint;

        if (nextWaypoint == null){
            return;
        }

        Vector2 direction = nextWaypoint.position - currentWaypoint.position;
        Flip(-direction.x);
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        if (IsGrounded() || currentWaypoint.type != WaypointType.Cliff) {
            rigidbody2D.linearVelocity = direction * chaseSpeed;
        }

        if (Vector2.Distance(enemyPosition, nextWaypoint.position) < 0.5f){
            if (nextWaypoint.type == WaypointType.Ladder) {
                transform.position = new Vector3(nextWaypoint.position.x, transform.position.y, 0);
            }
            aiAgent.AdvanceToNextWaypoint();
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
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackRange) {
            return false;
        }

        return Physics2D.Raycast(transform.position, direction, distance, obstaclesLayerMask).collider == null;
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

    /*public void ResetVelocity() {
        rigidbody2D.linearVelocity = Vector2.zero;
    }*/

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
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(currentWaypoint.position, 0.3f); // Dibuja el currentWaypoint

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
