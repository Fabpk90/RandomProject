using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class AStarGrid : MonoBehaviour {
    
        public Transform player;

        public Node[,] grid;

        public Vector2 gridSize;

        public float nodeRadius;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        public int MaxSize => gridSizeX * gridSizeY;

        public LayerMask unWalkableMask;
        public LayerMask walkableMask;

        public List<Node> path;
        public bool isGenerated;
        public PathFinding pathFinding;

        /// <summary>
        /// Callback to draw gizmos that are pickable and always drawn.
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position,new Vector3(gridSize.x,1,gridSize.y));

            if(grid != null)
            {
                foreach (var node in grid)
                {
                    if (node.walkable)
                    {
                        if(path != null && path.Contains(node))
                            Gizmos.color = Color.green;
                        else
                        {
                            Gizmos.color = Color.white;
                        }
                        
                        Gizmos.DrawSphere(node.position, (nodeRadius));
                    }
                }
            }
        }

        public void CreateGrid()
        {
            nodeDiameter = nodeRadius + nodeRadius;

            gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

            grid = new Node[gridSizeX, gridSizeY];

            Vector3 bottomLeftWorld =
                transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
            
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = bottomLeftWorld + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                         Vector3.forward * (y * nodeDiameter + nodeRadius);

                    bool walkable = Physics.CheckSphere(worldPoint, nodeRadius, walkableMask.value);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }

            pathFinding.openSet = new Heap<Node>(MaxSize);
            isGenerated = true;
            
        }

        public List<Node> GetNeighboursNode(Node node)
        {
            List<Node> nodes = new List<Node>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(i != 0 || j != 0)
                    {
                        int x = node.xIndex + i;
                        int y = node.yIndex + j;

                        if(x >= 0 && x < gridSizeX
                                  && y >= 0 && y < gridSizeY)
                        {
                            nodes.Add(grid[x, y]);
                        }
                    }
                }
            }

            return nodes;
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridSize.x / 2) / gridSize.x;
            float percentY = (worldPosition.z + gridSize.y / 2) / gridSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            return grid[x, y];
        }
    }
}