using System;
using AStar;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;


    public AStarGrid grid;
    public PathFinding pathFinding;

    public Transform player;
    
    private void Awake()
    {
        Instance = this;
    }
}