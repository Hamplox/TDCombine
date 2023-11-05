using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Path : MonoBehaviour 
{
    public List<Transform> paths = new List<Transform>();

    // Start is called before the first frame update
    public void AddPath(Transform transform)
    {
        paths.Add(transform);
    }

    public void Remove(Transform transform)
    {
        paths.Remove(transform);
    }
}
