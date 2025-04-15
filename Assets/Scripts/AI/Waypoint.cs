using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Waypoint
{
    public Vector2 position { get; }
    public List<Waypoint> neighbors { get; } = new List<Waypoint>();
    public WaypointType type { get; }
    public Waypoint bestNextWaypoint;

    //Constructor
    public Waypoint(Vector2 position, WaypointType type) {
        this.position = position;
        this.type = type;
    }

    
}

