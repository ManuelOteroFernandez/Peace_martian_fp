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

    //public Waypoint parent {get; set;} = null;
    /*public float f {get; set;} //Suma de g + h - Coste total
    public float g {get; set;} //El coste empleado en llegar hasta este nodo desde el inicio
    public float h {get; set;} //El coste de llegar hasta el destino desde este nodo*/

    //Constructor
    public Waypoint(Vector2 position, WaypointType type) {
        this.position = position;
        this.type = type;
    }

    
}

