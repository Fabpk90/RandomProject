using System;
using System.Collections.Generic;
using AStar;
using UnityEngine;
using UnityEngine.Serialization;

public class PathFinding : MonoBehaviour {
    
    public Transform seeker;
    public Transform target;

    [FormerlySerializedAs("grid")] public AStarGrid aStarGrid;
    
    private Node targetNode;
    public Heap<Node> openSet;
    

   /* private void Update() {
        if (aStarGrid.isGenerated)
        {
            //print("looking for a path..");
            FindPath(seeker.position, target.position);
        }
    }*/

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = aStarGrid.NodeFromWorldPoint(startPos);
        targetNode = aStarGrid.NodeFromWorldPoint(targetPos);

        openSet.currentItemCount = 0;
        HashSet<Node> closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            
            if(currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
                //print("Path found");
                
            }

            var list = aStarGrid.GetNeighboursNode(currentNode);
            foreach (var node in list)
            {
                if (!node.walkable || closedSet.Contains(node)) continue;
                var newCost = currentNode.gCost + GetDistance(currentNode, node);

                if (newCost >= currentNode.gCost && openSet.Contains(node)) continue;
                
                node.gCost = newCost;
                node.hCost = GetDistance(node, targetNode);

                node.parent = currentNode;

                if (!openSet.Contains(node))
                {
                    openSet.Add(node);
                }
                else
                    openSet.UpdateItem(node);
            }
        }

        return null;

        //print("Path not found");
    }


    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        path.Reverse();

        aStarGrid.path = path;

        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.xIndex - nodeB.xIndex);
        int distanceY = Mathf.Abs(nodeA.yIndex - nodeB.yIndex);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY); 

        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    public List<Node> FindPathToPlayer(Vector3 position)
    {
        return FindPath(position, GameManager.Instance.player.position);
    }
}