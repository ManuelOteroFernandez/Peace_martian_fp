using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private WaypointGenerator waypointGenerator;
    public List<Waypoint> path {get; private set;} = new List<Waypoint>();
    public int currentWpIndex {get; set;} = 0;
    public Waypoint currentWaypoint {get; set;}

    
    private void Start() {
        InvokeRepeating(nameof(UpdatePath), 1f, 1f); // 🔥 Recalcula la ruta cada 1 segundo
    }

    private void UpdatePath() {
        if (player == null || waypointGenerator == null) {
            return;
        }

        if (transform != null && player != null) {
            //path = waypointGenerator.GetGraph.FindPath(transform, player);
            currentWpIndex = 0;
            currentWaypoint = path[currentWpIndex];
        }
    }

    public void AdvanceToNextWaypoint() {
        if (currentWpIndex < path.Count - 1) {
            currentWpIndex++;
            currentWaypoint = path[currentWpIndex];
        }
    }

    private void OnDrawGizmos() {
        if (path.Count == 0){
            return;
        } 

        Gizmos.color = Color.red;
        
        for (int i = 0; i < path.Count - 1; i++) {
            Gizmos.DrawLine(path[i].position, path[i + 1].position);
        }
    }
}
