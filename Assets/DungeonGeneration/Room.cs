using System;
using UnityEngine;

public enum Direction
{
    UP = 0,
    RIGHT,
    DOWN,
    LEFT
};

public class Room : MonoBehaviour {
    

    public GameObject[] anchorPoints;

    public bool[] anchorPointsOccupied = new bool[4];

    public bool areAllAnchorPointsOccupied()
    {
        return anchorPointsOccupied[0] && anchorPointsOccupied[1] 
        && anchorPointsOccupied[2] && anchorPointsOccupied[3];
    }
}