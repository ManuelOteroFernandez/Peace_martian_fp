using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class AStarWaypoint
{
    public Vector2 position { get; }
    public List<AStarWaypoint> neighbors { get; } = new List<AStarWaypoint>();
    public bool isClimbable { get; }
    public bool isCliffNode { get; set; }
    public int groupId { get; set; } = -1;

    //public Waypoint parent {get; set;} = null;
    /*public float f {get; set;} //Suma de g + h - Coste total
    public float g {get; set;} //El coste empleado en llegar hasta este nodo desde el inicio
    public float h {get; set;} //El coste de llegar hasta el destino desde este nodo*/

    //Constructor
    public AStarWaypoint(Vector2 position, bool isClimbable, bool isCliffNode) {
        this.position = position;
        this.isClimbable = isClimbable;
        this.isCliffNode = isCliffNode;
    }
}

