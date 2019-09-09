using UnityEngine;

public class Node : IHeapItem<Node> {
    
    public bool walkable;

    public Vector3 position; 

    private int heapIndex;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get => gCost + hCost;
    }

    public int xIndex;
    public int yIndex;

    public Node parent;

    public int HeapIndex { get => heapIndex;
        set => heapIndex = value;
    }

    public Node(bool walkable, Vector3 position, int xIndex, int yIndex)
    {
        this.walkable = walkable;
        this.position = position;

        this.xIndex = xIndex;
        this.yIndex = yIndex;
    }

    public int CompareTo(Node nodeToCompare) 
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    public override string ToString()
    {
        return position.ToString();
    }
}