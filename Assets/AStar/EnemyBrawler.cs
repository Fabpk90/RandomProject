using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrawler : MonoBehaviour
{
    private Node playerNode;

    private List<Node> path;
    
    // Start is called before the first frame update
    void Start()
    {
        path = GameManager.Instance.pathFinding.FindPathToPlayer(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count != 0)
        {
            if (transform.position == path[0].position)
            {
                path.RemoveAt(0);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, path[0].position, Time.deltaTime);
            }
        }
        
    }
}
