using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaypointGenerator : MonoBehaviour {
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap ladderTilemap;
    [SerializeField] Transform player;
    Collider2D mapLimits;
    [SerializeField] private float gridSize = 1f;
    private WaypointGraph graph = new WaypointGraph();
    public WaypointGraph GetGraph => graph;

    private void Awake() {
        mapLimits = GetComponent<Collider2D>();
        GenerateGroundWaypoints();
        UpdatePath();
        InvokeRepeating(nameof(UpdatePath), 1f, 1f);
    }

    void UpdatePath() {
        //graph.CreateFlowField(player);
        graph.ComputeFlowField(player, groundLayer);
    }

    // Genera los waypoints necesarios para el desplazamiento de enemigos.
    // Se crea un nodo encima de cada tile de suelo siempre que dicha posición no esté ocupada por otro tile.
    // Se crea un nodo en cada tile de escalera.
    // Se crea un nodo encima de un tile de escalera si el tile superior está vacío.
    private void GenerateGroundWaypoints() {
        BoundsInt bounds = groundTilemap.cellBounds; // Obtiene los límites del Tilemap
        
        for (int x = bounds.xMin; x < bounds.xMax; x++) {
            for (int y = bounds.yMin; y < bounds.yMax; y++) {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Vector2 worldPos = groundTilemap.CellToWorld(tilePos) + (Vector3)(gridSize * 0.5f * Vector2.one); // Ajusta el centro

                if (groundTilemap.HasTile(tilePos)) {
                    // Si el tile superior está vacío, se crea un nodo encima
                    Vector3Int aboveTilePos = new Vector3Int(x, y + 1, 0);
                    if (!groundTilemap.HasTile(aboveTilePos) && !ladderTilemap.HasTile(aboveTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize, WaypointType.Ground);
                    }

                    Vector3Int leftTilePos = new Vector3Int(x - 1, y, 0);
                    Vector3Int rightTilePos = new Vector3Int(x + 1, y, 0);

                    // Creamos un nodo en el saliente de una plataforma. Se comprueban salientes a la izquierda y a la derecha
                    if (!groundTilemap.HasTile(aboveTilePos) && !groundTilemap.HasTile(leftTilePos) && !ladderTilemap.HasTile(leftTilePos)) {
                        Vector2 cliffPos = worldPos + Vector2.up * gridSize + Vector2.left * gridSize;
                        if(!IsInsideBounds(cliffPos)) continue;
                        graph.AddWaypoint(cliffPos, WaypointType.Cliff);
                    }

                    if (!groundTilemap.HasTile(aboveTilePos) && !groundTilemap.HasTile(rightTilePos) && !ladderTilemap.HasTile(rightTilePos)) {
                        Vector2 cliffPos = worldPos + Vector2.up * gridSize + Vector2.right * gridSize;
                        if(!IsInsideBounds(cliffPos)) continue;
                        graph.AddWaypoint(cliffPos, WaypointType.Cliff);
                    }
                }

                // Si el tile es una escalera, se crea un nodo en su posición
                if (ladderTilemap.HasTile(tilePos)) {
                    graph.AddWaypoint(worldPos, WaypointType.Ladder);

                    // Si la escalera tiene un tile vacío arriba, se crea otro nodo para indicar que se puede subir
                    Vector3Int aboveTilePos = new Vector3Int(x, y + 1, 0);
                    if (!groundTilemap.HasTile(aboveTilePos) && !ladderTilemap.HasTile(aboveTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize, WaypointType.Ground);
                    }
                }
            }
        }

        //Crear los waypoint que faltan para conectar las distintas secciones
        foreach(Waypoint w in graph.GetWaypointOfType(WaypointType.Cliff)) {
            Vector2 belowPosition = w.position - Vector2.up * gridSize;
            bool isWaypointBelowPosition = graph.FindWaypoint(belowPosition) != null;
            
            while (!isWaypointBelowPosition) {
                graph.AddWaypoint(belowPosition, WaypointType.Cliff);
                belowPosition -= Vector2.up * gridSize;
                isWaypointBelowPosition = graph.FindWaypoint(new Vector2(belowPosition.x, belowPosition.y)) != null;
            }
        }

        ConnectWaypoints();
    }

    private bool IsInsideBounds(Vector2 position){
        if (mapLimits == null){
            return false;
        }

        Bounds bounds = mapLimits.bounds;

        return position.x > bounds.min.x && position.x < bounds.max.x && position.y > bounds.min.y && position.y < bounds.max.y;
    }

    private void ConnectWaypoints() {
        foreach (var node in graph.waypoints) {
            Vector2 position = node.position;

            //Conectar waypoints horizontalmente
            Waypoint rightWaypoint = graph.FindWaypoint(position + Vector2.right * gridSize);
            if (rightWaypoint != null && (node.type != WaypointType.Cliff || rightWaypoint.type != WaypointType.Cliff)){
                rightWaypoint.neighbors.Add(node);
            }

            Waypoint leftWaypoint = graph.FindWaypoint(position + Vector2.left * gridSize);
            if (leftWaypoint != null && (node.type != WaypointType.Cliff || leftWaypoint.type != WaypointType.Cliff)){
                leftWaypoint.neighbors.Add(node);
            }

            //Conectar waypoints verticalmente
            graph.FindWaypoint(position + Vector2.up * gridSize)?.neighbors.Add(node);
            graph.FindWaypoint(position + Vector2.down * gridSize)?.neighbors.Add(node);
        }
    }

    /*=============TESTING=============*/
    private void OnDrawGizmos() {
        if (graph.waypoints == null){
            return;
        }

        foreach (var waypoint in graph.waypoints) {
            if (waypoint.type == WaypointType.Ground) {
                Gizmos.color = Color.blue;
            } else if (waypoint.type == WaypointType.Ladder) {
                Gizmos.color = Color.yellow;
            } else if (waypoint.type == WaypointType.Cliff) {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawSphere(waypoint.position, 0.1f);

            foreach (var neighbor in waypoint.neighbors) {
                Gizmos.DrawLine(waypoint.position, neighbor.position);
            }

            if (waypoint.bestNextWaypoint != null) {
                DrawArrow(waypoint.position, waypoint.bestNextWaypoint.position);
            }

            #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(
            waypoint.position + Vector2.down * 0.5f,
            $"({waypoint.position.x:0.00}, {waypoint.position.y:0.00})"
        );
#endif
        }
    }

    private void DrawArrow(Vector2 from, Vector2 to){
        Gizmos.color = Color.green;
        Gizmos.DrawLine(from, to);

        Vector2 direction = (to - from).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * 0.1f;

        Vector2 arrowHead1 = to - direction * 0.2f + perpendicular;
        Vector2 arrowHead2 = to - direction * 0.2f - perpendicular;

        Gizmos.DrawLine(to, arrowHead1);
        Gizmos.DrawLine(to, arrowHead2);
    }
}