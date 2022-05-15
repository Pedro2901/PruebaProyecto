using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Manager<GridManager>
{

    //public GameObject terrainGrid;
    public Tilemap grid;
    Dictionary<Team, int> StartPositionPerTeam;
    //List<Tile> allTiles = new List<Tile>();

    Graph graph;
    private void Awake()
    {
        base.Awake();

        InitializeGraph();
        StartPositionPerTeam = new Dictionary<Team, int>();
        StartPositionPerTeam.Add(Team.team1, 0);
        StartPositionPerTeam.Add(Team.team2, graph.nodes.Count - 1);

    }

    public Node GetfreeNode(Team forTeam)
    {
        int startIndex = StartPositionPerTeam[forTeam];
        int currentIndex = startIndex;

        while (graph.nodes[currentIndex].IsOccupied)
        {
            if (startIndex == 0)
            {
                currentIndex++;
                if (startIndex == graph.nodes.Count)
                {
                    return null;
                }
            }
            else
            {
                currentIndex--;
                if (currentIndex == -1)
                {
                    return null;
                }
            }

        }
        return graph.nodes[currentIndex];
    }

    public List<Node> GetPath(Node from, Node to)
    {
        return graph.GetPath(from, to);
    }

    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }
    private void InitializeGraph()
    {
        graph = new Graph();

        for (int x = grid.cellBounds.xMin; x < grid.cellBounds.xMax; x++)
        {
            for (int y = grid.cellBounds.yMin; y < grid.cellBounds.yMax; y++)
            {
                Vector3Int LocalPosition = new Vector3Int(x, y, (int)grid.transform.position.y);

                if (grid.HasTile(LocalPosition))
                {
                    Vector3 WorldPosition = grid.CellToWorld(LocalPosition);
                    graph.AddNode(WorldPosition);
                }
            }

        }
        var allNodes = graph.nodes;
        foreach (Node from in allNodes)
        {
            foreach (Node to in allNodes)
            {
                if (Vector3.Distance(from.WorldPosition, to.WorldPosition) <= 2f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }

    public int fronIndex = 0;
    public int toIndex = 0;
    private void OnDrawGizmos()
    {
        if (graph == null)
        {
            return;
        }

        var allEdges = graph.edges;
        Debug.Log("Aristas"+allEdges);
        foreach (Edge e in allEdges)
        {
            Debug.DrawLine(e.from.WorldPosition, e.to.WorldPosition, Color.black, 1);
        }
        var allNodes = graph.nodes;
        foreach (Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.WorldPosition, 0.1f);
        }
        Debug.Log("Numero de nodos: "+allNodes.Count);

        if (fronIndex >= allNodes.Count || toIndex >= allNodes.Count)
            return;

        List<Node> path = graph.GetPath(allNodes[fronIndex], allNodes[toIndex]);
        Debug.Log(path.Count);
        if (path.Count > 1)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].WorldPosition, path[i].WorldPosition, Color.red, 10);
                Debug.Log(path[i].WorldPosition);
            }
        }

    }
}