using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    //TODO Este campo no puede ser un SerializeField. Al ser un prefab necesita obtener una referencia al 
    //WaypointGenerator sin tener que asignarla desde el editor para poder instanciar nuevos enemigos en runtime.
    [SerializeField] private WaypointGenerator waypointGenerator;
    Waypoint currentWaypoint;

    //TODO: Cambiar como se está obteniendo FindClosestWaypoint. Tal y como está ahora puede provocar que se esté seleccionando un waypoint que se encuentre más arriba de lo deseado
    //Esto provoca que a veces el movimiento resulte en un movimiento negativo en el eje Y cuando queremos movimiento en el eje X. 
    //Por culpa de esto, el enemigo se queda bloqueado en secciones donde hay Ladders o Cliffs.
    private void Start() {
        currentWaypoint = waypointGenerator.GetGraph.FindClosestWaypoint(transform);
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
        currentWaypoint = waypointGenerator.GetGraph.FindClosestWaypoint(transform);
    }
}
