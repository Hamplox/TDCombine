using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GridTool : EditorWindow
{
    static GameObject gridCube;

    [MenuItem("Grid/Generate Grid")]
    public static void ShowWindow()
    {
        gridCube = Resources.Load<GameObject>("Assets/GridCube.prefab");
        GridTool gridTool = GetWindow<GridTool>();
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Generate Grid"))
        {
            gridCube = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Tool Assets/Grid Prefab/GridCube.prefab");
            if (gridCube == null)
                return;

            GenerateGrid();
        }

        if (GUILayout.Button("Remove Grid"))
        {
            RemoveGrid();
        }

    }

    public void ClearGrid()
    {
        RemoveGrid();
        MakeGrid();
    }

   public void MakeGrid()
    {
        gridCube = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Tool Assets/Grid Prefab/GridCube.prefab");
        if (gridCube == null)
            return;

       GenerateGrid();
    }

    private void GenerateGrid()
    {
        Vector2 minPos = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxPos = new Vector2(float.MinValue, float.MinValue);
        float height = 0;

        GameObject min = null;
        GameObject max = null;

        MeshFilter[] Meshes = GameObject.FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);
        foreach (var mesh in Meshes)
        {
            Transform transform = mesh.transform;
            if (transform.position.x < minPos.x)
            {
                minPos.x = transform.position.x;
                min = transform.gameObject;
            }
            if (transform.position.z < minPos.y)
            {
                minPos.y = transform.position.z;
                min = transform.gameObject;
            }

            if (transform.position.x > maxPos.x)
            {
                maxPos.x = transform.position.x;
                max = transform.gameObject;
            }
            if (transform.position.z > maxPos.y)
            {
                maxPos.y = transform.position.z;
                max = transform.gameObject;
            }

            if(transform.position.y > height)
            {
                height = transform.position.y;
            }
        }

        InstantiateGrid(minPos, maxPos, height);
    }

    private void InstantiateGrid(Vector2 aMinPos, Vector2 aMaxPos, float aHeight)
    {
        GameObject parent = new GameObject("Grid");
        parent.AddComponent<Path>();

        float cellSize = gridCube.transform.localScale.x;
        int gridSizeX = Mathf.CeilToInt((aMaxPos.x - aMinPos.x + cellSize) / cellSize);
        int gridSizeY = Mathf.CeilToInt((aMaxPos.y - aMinPos.y + cellSize) / cellSize);

        for (int y = 0; y < gridSizeY; ++y)
        {
            for (int x = 0; x < gridSizeX; ++x)
            {
                float xPos = aMinPos.x + x * cellSize;
                float yPos = aMinPos.y + y * cellSize;
                Vector3 finalPos = new Vector3(xPos, aHeight + 10, yPos);
                
                GameObject instantOBJ = Instantiate(gridCube, finalPos, new Quaternion());
                instantOBJ.transform.parent = parent.transform;
            }
        }
    }

    private void RemoveGrid()
    {
        GameObject parent = GameObject.Find("Grid");
        if (parent == null)
            return;

        GameObject[] children = new GameObject[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < children.Length; ++i)
        {
            DestroyImmediate(children[i]);
        }
        DestroyImmediate(parent.gameObject);
    }
}
