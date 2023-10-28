using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    private List<Transform> path;
    [SerializeField]
    private float speed;

    private int currentNode = 1;
    private float distanceToNextNode;


    // Start is called before the first frame update
    void Start()
    {

    }

    public bool RunPathfinder()
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

            if (isAtNode())
            {
                if (currentNode < path.Count - 1)
                    gameObject.transform.rotation = Quaternion.LookRotation(path[currentNode + 1].position - transform.position);

                currentNode++;
            }
        }
        else
        {
            currentNode = path.Count - 1;

            if (isAtNode())
            {
                return true;
            }
        }
        return false;
    }

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // Catmull-Rom equation
        return 0.5f * ((2 * p1) + (-p0 + p2) * t + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t + (-p0 + 3 * p1 - 3 * p2 + p3) * t * t * t);
    }

    private bool isAtNode()
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
