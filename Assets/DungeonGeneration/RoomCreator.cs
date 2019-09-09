using System.Collections;
using AStar;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomCreator : MonoBehaviour {
    
    public int roomNumber;
    public GameObject roomGO;

    public GameObject roomParent;
    [FormerlySerializedAs("grid")] public AStarGrid aStarGrid;

    public GameObject brawler;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(CreateRoomsCoroutine());
    }

    IEnumerator CreateRoomsCoroutine()
    {
        CreateRooms();
        
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        Vector2Int roomsSize = GetRoomsSize();
        aStarGrid.gridSize = roomsSize;

        aStarGrid.CreateGrid();

        Instantiate(brawler);
    }

    private Vector2Int GetRoomsSize()
    {
        Vector2Int roomPoints = Vector2Int.zero;

        for (int i = 0; i < roomParent.transform.childCount; i++)
        {
            var child = roomParent.transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                var size = child.GetChild(j).GetComponent<Renderer>().bounds.size;
                if(roomPoints.x <  Mathf.Abs(child.GetChild(j).transform.position.x) + size.x)
                {
                    roomPoints.x = (int) (Mathf.Abs(child.GetChild(j).transform.position.x) + size.x);
                }

                if (roomPoints.y < Mathf.Abs(child.GetChild(j).transform.position.z) + size.z)
                {
                    roomPoints.y = (int) (Mathf.Abs(child.GetChild(j).transform.position.z) + size.z);
                }
            }
        }

        return roomPoints * 2;
    }

    public void StartGenerating()
    {
        for(int i = roomParent.transform.childCount; i > 0; i--)
        {
            var child = roomParent.transform.GetChild(0);
            child.parent = null;

            Destroy(child.gameObject);
        }

        StartCoroutine(CreateRoomsCoroutine());       

    }

    private void CreateRooms()
    {
        //this is the main room
        Room previousRoom = Instantiate(roomGO, roomParent.transform).GetComponent<Room>();

        for (int i = 0; i < roomNumber - 1; i++)
        {
            Room room = Instantiate(roomGO, roomParent.transform).GetComponent<Room>();
            room.name = ""+i;

            Direction direction = Direction.UP;

            while (previousRoom.anchorPointsOccupied[(int)direction])
            {
                direction = (Direction) Random.Range((int)Direction.UP, (int)Direction.LEFT + 1);
                print("Searching for a point " + previousRoom.name);
                print("Searching for a point " + previousRoom.areAllAnchorPointsOccupied());
                print("Point on the try " + direction);
            }

            previousRoom.anchorPointsOccupied[(int)direction] = true;
            var bounds = room.GetComponent<MeshRenderer>().bounds;

            Vector3 position = Vector3.zero;

            switch (direction)
            {
                case Direction.DOWN:
                    position = previousRoom.anchorPoints[(int)direction].transform.position;
                    position.z -= bounds.size.z;
                break;
                case Direction.UP:
                    position = previousRoom.anchorPoints[(int)direction].transform.position;
                    position.z += bounds.size.z;
                break;
                case Direction.LEFT:
                    position = previousRoom.anchorPoints[(int)direction].transform.position;
                    position.x -= bounds.size.x;
                break;
                case Direction.RIGHT:
                    position = previousRoom.anchorPoints[(int)direction].transform.position;
                    position.x += bounds.size.x;
                break;
            }

            room.transform.position = position;

            if(Random.Range(0, 3) == 0 || previousRoom.areAllAnchorPointsOccupied())
            {
                previousRoom = room;
            }
        }
    }
}