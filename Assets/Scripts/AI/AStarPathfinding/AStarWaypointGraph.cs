using System.Collections.Generic;
using UnityEngine;

public class AStarWaypointGraph {
    public List<AStarWaypoint> waypoints {get; private set;} = new List<AStarWaypoint>();

    public void AddWaypoint(Vector2 position, bool isClimbable, bool isCliffNode) {
        AStarWaypoint node = new AStarWaypoint(position, isClimbable, isCliffNode);
        if (!waypoints.Contains(node)) {
            waypoints.Add(node);
        }
    }

    public AStarWaypoint FindWaypoint(Vector2 position) {
        foreach(AStarWaypoint n in waypoints) {
            if (n.position == position) {
                return n;
            }
        }

        return null;
    }

    //TODO: No funciona
    public List<AStarWaypoint> FindPath(Transform origin, Transform target) {
        if (origin == target) {
            return null;
        }

        AStarWaypoint startWaypoint = FindClosestWaypoint(origin);
        AStarWaypoint endWaypoint = FindClosestWaypointInGroup(target, startWaypoint.groupId);

        if (startWaypoint == null || endWaypoint == null) {
            return null;
        }

        List<AStarWaypoint> openList = new List<AStarWaypoint>();
        List<AStarWaypoint> closedList = new List<AStarWaypoint>();

        Dictionary<AStarWaypoint, float> gCost = new Dictionary<AStarWaypoint, float>();
        Dictionary<AStarWaypoint, float> hCost = new Dictionary<AStarWaypoint, float>();
        Dictionary<AStarWaypoint, float> fCost = new Dictionary<AStarWaypoint, float>();
        Dictionary<AStarWaypoint, AStarWaypoint> cameFrom = new Dictionary<AStarWaypoint, AStarWaypoint>();

        gCost.Add(startWaypoint, 0);
        hCost.Add(startWaypoint, Distance(startWaypoint, endWaypoint));
        fCost.Add(startWaypoint, hCost[startWaypoint]);

        openList.Add(startWaypoint);

        while (openList.Count > 0) {
            AStarWaypoint currentWaypoint = openList[0];

            for (int i = 1; i < openList.Count; i++) {
                if (fCost[openList[i]] < fCost[currentWaypoint] || fCost[openList[i]] == fCost[currentWaypoint] && hCost[openList[i]] < hCost[currentWaypoint]) {
                    currentWaypoint = openList[i];
                }
            }

            if (currentWaypoint == endWaypoint) {
                return ReconstructPath(cameFrom, startWaypoint, endWaypoint);
            }

            openList.Remove(currentWaypoint);
            closedList.Add(currentWaypoint);

            foreach(AStarWaypoint neighbor in currentWaypoint.neighbors) {
                if (closedList.Contains(neighbor)) {
                    continue;
                }

                float tentativeG = gCost[currentWaypoint] + Distance(currentWaypoint, neighbor);

                if (!openList.Contains(neighbor) || tentativeG < gCost[neighbor]) {
                    cameFrom[neighbor] = currentWaypoint;
                    gCost[neighbor] = tentativeG;
                    hCost[neighbor] = Distance(neighbor, endWaypoint);
                    fCost[neighbor] = gCost[neighbor] + hCost[neighbor];

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    public List<AStarWaypoint> ReconstructPath(Dictionary<AStarWaypoint, AStarWaypoint> cameFrom, AStarWaypoint startWaypoint, AStarWaypoint endWaypoint) {
        List<AStarWaypoint> pathList = new List<AStarWaypoint>(){
            endWaypoint
        };

        var p = cameFrom[endWaypoint];

        while (p != null && p != startWaypoint) {
            pathList.Insert(0, p);
            p = cameFrom[p];
        }

        pathList.Insert(0, startWaypoint);

        return pathList;
    }

    public AStarWaypoint FindClosestWaypoint(Transform target) {
        AStarWaypoint closest = null;
        foreach(AStarWaypoint waypoint in waypoints) {
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

    public AStarWaypoint FindClosestWaypointInGroup(Transform target, int groupId) {
        AStarWaypoint closest = null;
        foreach(AStarWaypoint waypoint in waypoints) {
            if (waypoint.groupId == groupId) {
                if (closest == null) {
                    closest = waypoint;
                } else {
                    if (Vector2.Distance(waypoint.position, target.position) < Vector2.Distance(closest.position, target.position)) {
                        closest = waypoint;
                    }
                }
            }
        }

        return closest;
    }

    float Distance(AStarWaypoint start, AStarWaypoint end){
        return Vector3.SqrMagnitude(start.position - end.position);
    }
}