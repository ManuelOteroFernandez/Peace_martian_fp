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
    Rigidbody2D rigidbody2D;
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

    public void MoveEnemy() {
        if (aiAgent.path.Count == 0 || aiAgent.currentWpIndex >= aiAgent.path.Count) {
            return;
        }

        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        if (Vector2.Distance(enemyPosition, aiAgent.path[aiAgent.currentWpIndex].position) < 0.5f) {
            aiAgent.AdvanceToNextWaypoint();

            if (aiAgent.path[aiAgent.currentWpIndex].type == WaypointType.Ladder) {
                transform.position = new Vector3(aiAgent.path[aiAgent.currentWpIndex].position.x, transform.position.y, transform.position.z);
            }
        }

        Vector2 direction = (aiAgent.currentWaypoint.position - enemyPosition).normalized;
        if (aiAgent.currentWaypoint.type == WaypointType.Ladder) {
            rigidbody2D.linearVelocity = new Vector2(0, direction.y * moveSpeed * Time.deltaTime);
        } else {
            rigidbody2D.linearVelocity = new Vector2(direction.x * moveSpeed * Time.deltaTime, rigidbody2D.linearVelocityY);
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemyPosition, 0.1f);
    }
}
