using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class AIAgent : MonoBehaviour {
    private WaypointGenerator waypointGenerator;
    public Waypoint currentWaypoint {get; set;}
    public Waypoint originWaypoint {get; set;}
    public Waypoint nextWaypoint {get; set;}
    List<Waypoint> patrolRoute;
    List<Waypoint> routeToOrigin;
    int patrolWaypointIndex = 0;
    public int routeToOriginWaypointIndex {get; private set;} = 0;
    [SerializeField] protected int patrolWaypointsRange = 2;

    private void Awake() {
        waypointGenerator = GameObject.FindGameObjectWithTag("WaypointManager")?.GetComponent<WaypointGenerator>();
    }
    //TODO: Cambiar como se está obteniendo FindClosestWaypoint. Tal y como está ahora puede provocar que se esté seleccionando un waypoint que se encuentre más arriba de lo deseado
    //Esto provoca que a veces el movimiento resulte en un movimiento negativo en el eje Y cuando queremos movimiento en el eje X. 
    //Por culpa de esto, el enemigo se queda bloqueado en secciones donde hay Ladders o Cliffs.
    //¿Corregido usando el spriteRenderer?
    public void Initialize() {
        currentWaypoint = waypointGenerator.GetGraph.FindClosestWaypoint(transform, transform.GetComponent<SpriteRenderer>());
        originWaypoint = currentWaypoint;
        GetPatrolRoute();
    }

    public void GetPatrolRoute() {
        List<Waypoint> groundWaypoints = waypointGenerator.GetGraph.waypoints
            .Where(w => w.type == WaypointType.Ground && w.position.y == originWaypoint.position.y)
            .OrderBy(w => w.position.x)
            .ToList();

        int currentIndex = groundWaypoints.FindIndex(w => w == originWaypoint);

        if (currentIndex == -1) {
            patrolRoute = new List<Waypoint>();
        } 

        Waypoint leftWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, false);;
        Waypoint rightWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, true);;

        patrolRoute = new List<Waypoint> { leftWaypoint, rightWaypoint };
    }

    private bool AreNeighbors(Waypoint a, Waypoint b) {
        return a.neighbors.Contains(b) || b.neighbors.Contains(a);
    }

    /// <summary>
    /// Busca el nodo que se encuentre a "range" nodos del "origin" e una "direction" dada y se encuentre conectado al origen. Si no existe ese nodo se devuelve el nodo más alejado en la dirección correspondiente.
    /// </summary>
    /// <param name="origin">Waypoint de origen</param>
    /// <param name="direction">Dirección en la que se busca el nodo más alejado. false = izquierda, true derecha</param>
    /// <param name="range">Distancia medida en waypoints</param>
    /// <returns></returns>
    private Waypoint FindFurthestWaypoint(List<Waypoint> waypoints, int currentIndex, bool directionRight) {
        Waypoint furthestWaypoint = currentWaypoint;
        for (int i = 1; i <= patrolWaypointsRange; i++)
        {
            int furthestIndex = directionRight ? currentIndex + i : currentIndex - i;
            if (furthestIndex < 0 || furthestIndex >= waypoints.Count || !AreNeighbors(waypoints[furthestIndex], furthestWaypoint)) {
                break;
            }
            furthestWaypoint = waypoints[furthestIndex];
        }

        return furthestWaypoint;
    }

    public void CalculateRouteToOrigin() {
        if (routeToOrigin == null) {
            routeToOrigin = waypointGenerator.GetGraph.FindPath(currentWaypoint, originWaypoint);
        }
    }

    public bool CanJumpFromCliff(Waypoint waypoint) {
        return waypoint.bestNextWaypoint.type == WaypointType.Ground;
    }

    public bool IsInOriginWaypoint() {
        return currentWaypoint == originWaypoint;
    }

    public float DistanceToOriginWaypoint() {
        return Vector2.Distance(currentWaypoint.position, originWaypoint.position);
    }

    public Vector2 DirectionToNextWaypoint() {
        return nextWaypoint.position - currentWaypoint.position;
    }

    public void AdvanceToNextWaypoint() {
        currentWaypoint = nextWaypoint;
    }

    public Vector2 GetCurrentDirection() {
        return currentWaypoint.position - currentWaypoint.bestNextWaypoint.position;
    }

    public void UpdateFieldFlowNextWaypoint() {
        nextWaypoint = currentWaypoint.bestNextWaypoint;
    }

    public void UpdateRouteToOriginNextWaypoint() {
        nextWaypoint = routeToOrigin[routeToOriginWaypointIndex];
    }

    public void UpdateNextPatrolWaypoint() {
        nextWaypoint = patrolRoute[patrolWaypointIndex];
    }

    public bool RouteIndexOutOfBounds() {
        return routeToOriginWaypointIndex >= routeToOrigin.Count;
    }

    public void IncreaseRouteToOriginWaypointIndex() {
        routeToOriginWaypointIndex++;
    }

    public void ResetRouteToOriginWaypointIndex() {
        routeToOriginWaypointIndex = 0;
    }

    public void ResetRouteToOrigin() {
        routeToOrigin = null;
        ResetRouteToOriginWaypointIndex();
    }

    public void UpdatePatrolWaypointIndex() {
        if (patrolWaypointIndex + 1 == patrolRoute.Count) {
            patrolWaypointIndex--;
        } else {
            patrolWaypointIndex++;
        }
    }

    public void UpdateOriginWaypoint(Waypoint waypoint) {
        originWaypoint = waypoint;
    }

    public void RelocateCurrentWaypoint() {
        currentWaypoint = waypointGenerator.GetGraph.FindClosestWaypoint(transform, transform.GetComponent<SpriteRenderer>());
    }

    private void OnDrawGizmos() {
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
}
