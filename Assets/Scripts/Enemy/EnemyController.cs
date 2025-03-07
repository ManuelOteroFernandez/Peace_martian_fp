using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("States")]
    EnemyState currentState;

    [Header("Components")]
    public Animator animator {get; private set;}
    public EnemyHealth enemyHealth {get; private set;}
    public AIAgent aiAgent {get; private set;}
    public Rigidbody2D rigidbody2D {get; private set;}
    SpriteRenderer spriteRenderer;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 1f;

    Vector2 enemyPosition;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAgent = GetComponent<AIAgent>();
    }

    void Start()
    {
        ChangeState(new EnemyIdleState());
    }

    void FixedUpdate()
    {
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

    public bool IsGrounded() {
        return !enemyHealth.IsInBubble() && rigidbody2D.linearVelocityY == 0;
    }

    //TODO: Necesita ajuste
    public void MoveEnemy(){
        Waypoint currentWaypoint = aiAgent.GetCurrentWaypoint();
        Waypoint nextWaypoint = currentWaypoint.bestNextWaypoint;

        if (nextWaypoint == null){
            return;
        }

        Vector2 direction = nextWaypoint.position - currentWaypoint.position;
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        if (IsGrounded() || currentWaypoint.type != WaypointType.Cliff) {
            rigidbody2D.linearVelocity = direction * moveSpeed;
        }

        if (Vector2.Distance(enemyPosition, nextWaypoint.position) < 0.5f){
            if (nextWaypoint.type == WaypointType.Ladder) {
                transform.position = new Vector3(nextWaypoint.position.x, transform.position.y, 0);
            }
            aiAgent.AdvanceToNextWaypoint();
        }
    }

    public void StartChasing() {
        aiAgent.RelocateCurrentWaypoint();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemyPosition, 0.1f);
    }
}
