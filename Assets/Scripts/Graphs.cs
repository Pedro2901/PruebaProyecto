using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
public List<Node>nodes;
public List<Edge>edges;

    public Graph()
    {
        nodes=new List<Node>();
        edges=new List<Edge>();
    }
public bool Adjacent(Node from, Node to)
    {
        foreach(Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return true;
        }
        return false;  
    }

    public List<Node> Neighbors(Node of)
    {
        List<Node> result = new List<Node>();

        foreach (Edge e in edges)
        {
            if (e.from == of)
                result.Add(e.to);
        }
        return result;
    }

    public void AddNode(Vector3 worldPosition)
    {
        nodes.Add(new Node(nodes.Count, worldPosition));
    }

    public void AddEdge(Node from, Node to)
    {
        edges.Add(new Edge(from, to, 1));
    }

    public float Distance(Node from, Node to)
    {
        foreach (Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return e.GetWeight();
              
        }

        return Mathf.Infinity;
    }
    
    public List<Node> GetPath(Node Start,Node end){
        List<Node> path = new List<Node>();
        if(Start==end){
        path.Add(Start);
        return path;
        }
        List<Node> openList = new List<Node>();
        Dictionary<Node,Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, float> distances = new Dictionary<Node, float>();
        for(int i=0; i< nodes.Count; i++){
            openList.Add(nodes[i]);
            distances.Add(nodes[i],float.PositiveInfinity);//Distancia por defecto es infinita

}
distances[Start]=0f;//distancia en el nodo es cero
while(openList.Count>0){
       //menor distancia hacia el nodo
    openList= openList.OrderBy(x=>distances[x]).ToList();
    Node current=openList[0];
    openList.Remove(current);
    
    if(current==end){

        while(previous.ContainsKey(current)){
        path.Insert(0,current);
        current=previous[current];
        }
    path.Insert(0,current);
    break;
   }
    foreach(Node neighbor in Neighbors(current))
    {
        float distance=Distance(current,neighbor);
        float candidateNewDistance=distances[current]+distance;
        //Hallar ruta mas corta
        if(candidateNewDistance<distances[neighbor]){
        distances[neighbor]= candidateNewDistance ;
        previous[neighbor]=current;
    }}

}
Debug.Log("retorno path");
return path;
}





}
public class Node{
    public int index;
    public Vector3 WorldPosition;
    private bool occupied = false;
    public bool IsOccupied => occupied;
    public Node(int index , Vector3 _WorldPosition){
        this.index = index;
        WorldPosition = _WorldPosition;
        occupied= false;
    }

    public void SetOccupied(bool value){
        occupied = value;
    }
}
public class Edge{
    public Node from ;
    public Node to ;

    public float weight;
    
    public Edge(Node from, Node to, float weight){
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public float GetWeight(){
        if(to.IsOccupied){
            return Mathf.Infinity;
        }
        return weight;
    }

}
