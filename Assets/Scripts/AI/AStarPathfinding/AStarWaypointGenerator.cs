using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarWaypointGenerator : MonoBehaviour
{
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap ladderTilemap;
    [SerializeField] private float gridSize = 1f;
    private AStarWaypointGraph graph = new AStarWaypointGraph();
    public AStarWaypointGraph GetGraph => graph;

    private void Start() {
        GenerateGroundWaypoints();
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

                // Si hay suelo en esta posición
                if (groundTilemap.HasTile(tilePos)) {
                    // Si el tile superior está vacío, se crea un nodo encima
                    Vector3Int aboveTilePos = new Vector3Int(x, y + 1, 0);
                    if (!groundTilemap.HasTile(aboveTilePos) && !ladderTilemap.HasTile(aboveTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize, false, false);
                    }

                    Vector3Int leftTilePos = new Vector3Int(x - 1, y, 0);
                    Vector3Int rightTilePos = new Vector3Int(x + 1, y, 0);

                    // Creamos un nodo en el saliente de una plataforma. Se comprueban salientes a la izquierda y a la derecha
                    if (!groundTilemap.HasTile(aboveTilePos) && !groundTilemap.HasTile(leftTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize + Vector2.left * gridSize, false, true);
                    }

                    if (!groundTilemap.HasTile(aboveTilePos) && !groundTilemap.HasTile(rightTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize + Vector2.right * gridSize, false, true);
                    }
                }

                // Si el tile es una escalera, se crea un nodo en su posición
                if (ladderTilemap.HasTile(tilePos)) {
                    graph.AddWaypoint(worldPos, true, false);

                    // Si la escalera tiene un tile vacío arriba, se crea otro nodo para indicar que se puede subir
                    Vector3Int aboveTilePos = new Vector3Int(x, y + 1, 0);
                    if (!groundTilemap.HasTile(aboveTilePos) && !ladderTilemap.HasTile(aboveTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize, false, false);
                    }

                    Vector3Int leftTilePos = new Vector3Int(x - 1, y, 0);
                    Vector3Int rightTilePos = new Vector3Int(x + 1, y, 0);

                    // Creamos un nodo en el saliente de una plataforma. Se comprueban salientes a la izquierda y a la derecha
                    if (!groundTilemap.HasTile(aboveTilePos) && !ladderTilemap.HasTile(aboveTilePos) && !groundTilemap.HasTile(leftTilePos) && !ladderTilemap.HasTile(leftTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize + Vector2.left * gridSize, false, true);
                    }

                    if (!groundTilemap.HasTile(aboveTilePos) && !ladderTilemap.HasTile(aboveTilePos) && !groundTilemap.HasTile(rightTilePos) && !ladderTilemap.HasTile(rightTilePos)) {
                        graph.AddWaypoint(worldPos + Vector2.up * gridSize + Vector2.right * gridSize, false, true);
                    }
                }
            }
        }

        ConnectWaypoints();
        GroupWaypoints();
    }

    private void ConnectWaypoints() {
        foreach (var node in graph.waypoints) {
            Vector2 position = node.position;

            //Conectar waypoints horizontalmente
            graph.FindWaypoint(position + Vector2.right * gridSize)?.neighbors.Add(node);
            graph.FindWaypoint(position + Vector2.left * gridSize)?.neighbors.Add(node);

            //Conectar waypoints verticalmente
            graph.FindWaypoint(position + Vector2.up * gridSize)?.neighbors.Add(node);
            graph.FindWaypoint(position + Vector2.down * gridSize)?.neighbors.Add(node);
        }
    }

    public void GroupWaypoints() {
        //Recorremos todos los nodos para asignarlos a un grupo en función de a los nodos que se encuentran conectados.
        int groupId = 0;
        HashSet<AStarWaypoint> visited = new HashSet<AStarWaypoint>();

        foreach (var waypoint in graph.waypoints) {
            if (!visited.Contains(waypoint)) {
                Stack<AStarWaypoint> stack = new Stack<AStarWaypoint>();
                stack.Push(waypoint);

                while (stack.Count > 0) {
                    AStarWaypoint current = stack.Pop();
                    if (visited.Contains(current)){
                        continue;
                    } 

                    visited.Add(current);
                    current.groupId = groupId;

                    foreach (var neighbor in current.neighbors){
                        if (!visited.Contains(neighbor)){
                            stack.Push(neighbor);
                        }
                    }
                }

                groupId++;
            }
        }
    }



    /*=============TESTING=============*/
    private void OnDrawGizmos() {
        Bounds b = GetComponent<Collider2D>().bounds;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(b.center, b.size);

        //Dibuja el área de detección del suelo para depuración
        foreach (AStarWaypoint w in graph.waypoints) {
            if (w.groupId == 0) {
                    Gizmos.color = Color.red;
                } else if (w.groupId == 1 ) {
                    Gizmos.color = Color.yellow;
                } else if (w.groupId == 2) {
                    Gizmos.color = Color.blue;
                } else if (w.groupId == 3) {
                    Gizmos.color = Color.magenta;
                } else if (w.groupId == 4) {
                    Gizmos.color = Color.cyan;
                } else if (w.groupId == 5) {
                    Gizmos.color = Color.grey;
                } else if (w.groupId == 6) {
                    Gizmos.color = Color.black;
                } else if (w.groupId == 7) {
                    Gizmos.color = Color.white;
                } else if (w.groupId == 8) {
                    Gizmos.color = Color.red;
                } else if (w.groupId == 9) {
                    Gizmos.color = Color.green;
                }
            Gizmos.DrawWireSphere(w.position, 0.1f);
            /*foreach (Waypoint n in w.neighbors) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(w.position, n.position);
            }*/
        }
    }
}
