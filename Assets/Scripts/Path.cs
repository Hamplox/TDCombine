using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Path : MonoBehaviour
{
    public static List<Transform> paths;

    // Start is called before the first frame update
    void Awake()
    {
        paths = new List<Transform>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            paths.Add(transform.GetChild(i));
        }
    }
}
