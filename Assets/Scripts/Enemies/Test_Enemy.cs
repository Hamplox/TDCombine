using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinder))]
public class Test_Enemy : MonoBehaviour
{
    private Pathfinder pathfinder;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = gameObject.GetComponent<Pathfinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pathfinder.RunPathfinder())
        {
            Destroy(gameObject);
        }
    }
}
