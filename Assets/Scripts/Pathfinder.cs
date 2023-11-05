using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private int currentNode = 1;
    private float distanceToNextNode;

    private List<Transform> path = new List<Transform>();
    private Transform startNode;
    private Transform endNode;

    private void Awake()
    {
        startNode = GameObject.FindGameObjectWithTag("StartWayPoint").transform;
        path.Add(startNode);
        path.AddRange(GameObject.Find("Grid").GetComponent<Path>().paths);
        endNode = GameObject.FindGameObjectWithTag("EndWayPoint").transform;
        path.Add(endNode);
    }

    public bool FindPathfinder(float speed)
    {
        if (currentNode <= path.Count - 1)
        {
            // Calculate the distance to the next node
            distanceToNextNode = Vector3.Distance(transform.position, path[currentNode].position);

            // Calculate the direction to the next node
            Vector3 directionToNextNode = (path[currentNode].position - transform.position).normalized;

            // Calculate the position to reach at the current frame
            Vector3 newPosition = transform.position + directionToNextNode * (speed * Time.deltaTime);

            // Check if the newPosition will pass the target node, and if so, set it to the target node
            if (Vector3.Distance(newPosition, path[currentNode].position) < distanceToNextNode)
            {
                newPosition.y = transform.position.y;
                transform.position = newPosition;
            }
            else
            {
                // If the newPosition passes the target node, move directly to the next node
                Vector3 oldPos = transform.position;
                oldPos = path[currentNode].position;
                oldPos.y = transform.position.y;
                transform.position = oldPos;

            }

            if (isAtNode(path))
            {
                if (currentNode < path.Count - 1)
                    gameObject.transform.rotation = Quaternion.LookRotation(path[currentNode + 1].position - transform.position);

                currentNode++;
            }
        }
        else
        {
            currentNode = path.Count - 1;

            if (isAtNode(path))
            {
                return true;
            }
        }

        return false;
    }

    public bool RunPathfinder(float speed)
    {
        if (currentNode <= path.Count - 1)
        {
            // Calculate the distance to the next node
            distanceToNextNode = Vector3.Distance(transform.position, path[currentNode].position);

            // Calculate the direction to the next node
            Vector3 directionToNextNode = (path[currentNode].position - transform.position).normalized;

            // Calculate the position to reach at the current frame
            Vector3 newPosition = transform.position + directionToNextNode * (speed * Time.deltaTime);

            // Check if the newPosition will pass the target node, and if so, set it to the target node
            if (Vector3.Distance(newPosition, path[currentNode].position) < distanceToNextNode)
            {
                newPosition.y = transform.position.y;
                transform.position = newPosition;
            }
            else
            {
                // If the newPosition passes the target node, move directly to the next node
                Vector3 oldPos = transform.position;
                oldPos = path[currentNode].position;
                oldPos.y = transform.position.y;
                transform.position = oldPos;

            }

            if (isAtNode(path))
            {
                if (currentNode < path.Count - 1)
                    gameObject.transform.rotation = Quaternion.LookRotation(path[currentNode + 1].position - transform.position);

                currentNode++;
            }
        }
        else
        {
            currentNode = path.Count - 1;

            if (isAtNode(path))
            {
                return true;
            }
        }
        return false;
    }

    private bool isAtNode(List<Transform> path)
    {
        Vector2 currentPos = gameObject.transform.position;
        currentPos.y = gameObject.transform.position.z;
        currentPos.x = gameObject.transform.position.x;

        Vector2 toPos = path[currentNode].position;
        toPos.y = path[currentNode].position.z;
        toPos.x = path[currentNode].position.x;

        if (Vector2.Distance(currentPos, toPos) < 0.5f)
        {
            return true;
        }

        return false;
    }

}
