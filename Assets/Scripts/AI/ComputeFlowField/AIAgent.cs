using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIAgent : MonoBehaviour {
    private WaypointGenerator waypointGenerator;
    Waypoint currentWaypoint;

    private void Awake() {
        waypointGenerator = GameObject.FindGameObjectWithTag("WaypointManager")?.GetComponent<WaypointGenerator>();
    }
    //TODO: Cambiar como se está obteniendo FindClosestWaypoint. Tal y como está ahora puede provocar que se esté seleccionando un waypoint que se encuentre más arriba de lo deseado
    //Esto provoca que a veces el movimiento resulte en un movimiento negativo en el eje Y cuando queremos movimiento en el eje X. 
    //Por culpa de esto, el enemigo se queda bloqueado en secciones donde hay Ladders o Cliffs.
    //¿Corregido usando el spriteRenderer?
    private void Start() {
        currentWaypoint = waypointGenerator.GetGraph.FindClosestWaypoint(transform, transform.GetComponent<SpriteRenderer>());
    }

    public void AdvanceToNextWaypoint() {
        currentWaypoint = currentWaypoint.bestNextWaypoint;
    }

    public Vector2 GetCurrentDirection() {
        return currentWaypoint.position - currentWaypoint.bestNextWaypoint.position;
    }

    public Waypoint GetCurrentWaypoint() {
        return currentWaypoint;
    }

    public void RelocateCurrentWaypoint() {
        currentWaypoint = waypointGenerator.GetGraph.FindClosestWaypoint(transform, transform.GetComponent<SpriteRenderer>());
    }

    public List<Waypoint> GetPatrolRoute(int range) {
        List<Waypoint> groundWaypoints = waypointGenerator.GetGraph.waypoints
            .Where(w => w.type == WaypointType.Ground && w.position.y == currentWaypoint.position.y)
            .OrderBy(w => w.position.x)
            .ToList();

        int currentIndex = groundWaypoints.FindIndex(w => w == currentWaypoint);

        if (currentIndex == -1) {
            return new List<Waypoint>();
        } 

        Waypoint leftWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, false, range);
        Waypoint rightWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, true, range);

        return new List<Waypoint> { leftWaypoint, rightWaypoint };
    }

        public List<Waypoint> GetPatrolRoute(Waypoint originWaypoint, int range) {
        List<Waypoint> groundWaypoints = waypointGenerator.GetGraph.waypoints
            .Where(w => w.type == WaypointType.Ground && w.position.y == originWaypoint.position.y)
            .OrderBy(w => w.position.x)
            .ToList();

        int currentIndex = groundWaypoints.FindIndex(w => w == originWaypoint);

        if (currentIndex == -1) {
            return new List<Waypoint>();
        } 

        Waypoint leftWaypoint = null;
        if (currentIndex > 0) {
            leftWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, false, range);
        } else {
            leftWaypoint = groundWaypoints[0];
        }

        Waypoint rightWaypoint = null;
        if (currentIndex < groundWaypoints.Count - 1) {
            rightWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, true, range);
        } else {
            rightWaypoint = groundWaypoints[groundWaypoints.Count - 1];
        }
        /*Waypoint leftWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, false, range);
        Waypoint rightWaypoint = FindFurthestWaypoint(groundWaypoints, currentIndex, true, range);*/

        return new List<Waypoint> { leftWaypoint, rightWaypoint };
    }

    public List<Waypoint> FindRouteToWaypoint(Waypoint targetWaypoint) {
        return waypointGenerator.GetGraph.FindPath(currentWaypoint, targetWaypoint);
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
    private Waypoint FindFurthestWaypoint(List<Waypoint> waypoints, int currentIndex, bool directionRight, int range) {
        Waypoint furthestWaypoint = currentWaypoint;
        for (int i = 1; i <= range; i++)
        {
            int furthestIndex = directionRight ? currentIndex + i : currentIndex - i;
            if (furthestIndex < 0 || !AreNeighbors(waypoints[furthestIndex], furthestWaypoint)) {
                break;
            }
            furthestWaypoint = waypoints[furthestIndex];
        }

        return furthestWaypoint;
    }

    public bool CanJumpFromCliff(Waypoint waypoint) {
        return waypoint.bestNextWaypoint.type == WaypointType.Ground;
    }
}
