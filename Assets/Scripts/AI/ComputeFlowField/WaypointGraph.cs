using System.Collections.Generic;
using UnityEngine;

public class WaypointGraph {
    public List<Waypoint> waypoints {get; private set;} = new List<Waypoint>();

    public void AddWaypoint(Vector2 position, WaypointType type) {
        Waypoint node = new Waypoint(position, type);
        if (!waypoints.Contains(node)) {
            waypoints.Add(node);
        }
    }

    public Waypoint FindWaypoint(Vector2 position) {
        foreach(Waypoint n in waypoints) {
            if (n.position == position) {
                return n;
            }
        }

        return null;
    }

    //Genera el flow field usando los nodos del mapa, siguiendo las siguientes reglas:
    //1.- Entre waypoints de tipo Ground puede moverse en cualquier sentido.
    //2.- Entre waypoints de tipo Cliff solo se puede mover hacia abajo.
    //3.- Entre waypoints de tipo Ladder se puede mover arriba y abajo.
    //4.- El objetivo es el nodo accesible más cercano al targetTransform.
    public void ComputeFlowField(Transform targetTransform, LayerMask groundLayer) {
        Waypoint originalTarget = FindClosestWaypoint(targetTransform);
        Waypoint target = GetCorrectedTarget(originalTarget);
        foreach (var waypoint in waypoints) {
            waypoint.bestNextWaypoint = null;
        }

        Queue<Waypoint> queue = new Queue<Waypoint>();
        queue.Enqueue(target);

        while (queue.Count > 0) {
            Waypoint current = queue.Dequeue();

            foreach (var neighbor in current.neighbors) {
                if (current.type == WaypointType.Ground && neighbor.type == WaypointType.Ground) {
                    if (!CanMoveHorizontally(current, neighbor, groundLayer))  {
                        Waypoint closestLadder = FindClosestLadder(current);
                        if (closestLadder != null) {
                            neighbor.bestNextWaypoint = closestLadder;
                            queue.Enqueue(closestLadder);
                            continue;
                        }
                    }
                }

                if (neighbor.type == WaypointType.Cliff && current.position.y > neighbor.position.y){
                    continue;
                }

                if (neighbor.bestNextWaypoint == null) {
                    neighbor.bestNextWaypoint = current;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    private bool IsNotAccesibleWaypoint(Waypoint waypoint) {
        bool hasCliffLeft = false;
        bool hasCliffRight = false;

        List<Waypoint> openWaypoints = new List<Waypoint>();
        List<Waypoint> closedWaypoints = new List<Waypoint>();

        openWaypoints.Add(waypoint);

        if (waypoint.type == WaypointType.Cliff) {
            return true;
        }

        while (openWaypoints.Count > 0) {
            Waypoint checkWaypoint = openWaypoints[0];

            openWaypoints.Remove(checkWaypoint);
            closedWaypoints.Add(checkWaypoint);

            if (checkWaypoint.position.y == waypoint.position.y) {
                if (checkWaypoint.type == WaypointType.Cliff) {
                    if (checkWaypoint.position.x > waypoint.position.x) {
                        hasCliffRight = true;
                    } else if (checkWaypoint.position.x < waypoint.position.x) {
                        hasCliffLeft = true;
                    }
                } else {
                    foreach(Waypoint w in checkWaypoint.neighbors) {
                        if(!closedWaypoints.Contains(w)) {
                            openWaypoints.Add(w);
                        }
                    }
                }
            }
        }

        return hasCliffLeft && hasCliffRight;
    }

    //Se busca el nodo que se encuentre en la misma coordenada Y pero más abajo.
    private Waypoint FindWaypointBelow(Waypoint waypoint) {
        Waypoint closestBelow = null;
        float minDistance = float.MaxValue;

        foreach (var other in waypoints) {
            if (other.position.x == waypoint.position.x && other.position.y < waypoint.position.y) {
                float distance = waypoint.position.y - other.position.y;
                if (distance < minDistance) {
                    minDistance = distance;
                    closestBelow = other;
                }
            }
        }

        return closestBelow;
    }

    //Si el waypoint no es accesible por lo enemigos, se busca cual es el nodo que esté debajo que si lo sea.
    public Waypoint GetCorrectedTarget(Waypoint waypoint) {
        if (!IsNotAccesibleWaypoint(waypoint)){
            return waypoint;
        } 

        Waypoint correctedWaypoint = FindWaypointBelow(waypoint);

        if (correctedWaypoint != null && !IsNotAccesibleWaypoint(correctedWaypoint)) {
            return correctedWaypoint;
        }

        while (correctedWaypoint != null) {
            Waypoint next = FindWaypointBelow(correctedWaypoint);
            if (next == null || !IsNotAccesibleWaypoint(next)){
                return next;    
            }

            correctedWaypoint = next;
        }

        return waypoint;
    }

    //Comprueba si se puede seguir moviendo horizontalmente sin encontrar una pared entre 2 nodos dados.
    private bool CanMoveHorizontally(Waypoint from, Waypoint to, LayerMask groundLayer){
        if (Physics2D.Linecast(from.position, to.position, groundLayer)) {
            return false; 
        }

        return true;
    }

    private Waypoint FindClosestLadder(Waypoint current) {
        Waypoint closestLadder = null;
        float minDistance = float.MaxValue;

        foreach (var waypoint in waypoints) {
            if (waypoint.type == WaypointType.Ladder) {
                float distance = Vector2.Distance(current.position, waypoint.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    closestLadder = waypoint;
                }
            }
        }

        return closestLadder;
    }

    public Waypoint FindClosestWaypoint(Transform target) {
        Waypoint closest = null;
        foreach(Waypoint waypoint in waypoints) {
            if (closest == null) {
                closest = waypoint;
            } else {
                if (Vector2.Distance(waypoint.position, target.position) < Vector2.Distance(closest.position, target.position)) {
                    closest = waypoint;
                }
            }
        }

        return closest;
    }

    public List<Waypoint> GetWaypointOfType(WaypointType type) {
        List<Waypoint> waypointsOfType = new List<Waypoint>();

        foreach(Waypoint waypoint in waypoints) {
            if (waypoint.type == type) {
                waypointsOfType.Add(waypoint);
            }
        }

        return waypointsOfType;
    }
}