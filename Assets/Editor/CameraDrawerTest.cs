using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraDrawerTest : EditorWindow
{
    Camera camera;
    RenderTexture renderTexture;

    [MenuItem("Grid/Camera Drawer")]
    public static void ShowWindow()
    {
        GetWindow<CameraDrawerTest>("Camera View");
    }

    public void Awake()
    {
        renderTexture = new RenderTexture(1920, 1080, (int)RenderTextureFormat.ARGB32); // Set an initial aspect ratio (e.g., 16:9)
    }

    public void OnEnable()
    {
        var camera2 = GameObject.Find("Grid Tool Camera");
        if (camera2 == null)
        {
            var cam = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tool Assets/Paint Tool/Grid Tool Camera.prefab");
            PrefabUtility.InstantiatePrefab(cam);
            camera = GameObject.Find("Grid Tool Camera").GetComponent<Camera>();
        }
        else
        {
            camera = camera2.GetComponent<Camera>();
        }
    }

    private void OnDestroy()
    {
        DestroyImmediate(GameObject.Find("Grid Tool Camera"));
    }

    public void Update()
    {
        if (camera != null)
        {
            if (camera.targetTexture != renderTexture)
            {
                camera.targetTexture = renderTexture;
            }
            camera.Render();
            camera.targetTexture = null;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();

        if (camera != null)
        {
            // Calculate the Rect to maintain aspect ratio
            float aspect = (float)renderTexture.width / renderTexture.height;
            float windowAspect = position.width / position.height;
            float scaleFactor = aspect / windowAspect;
            Rect rect = new Rect(0, 0, position.width, position.height);

            if (scaleFactor < 1.0f)
            {
                rect.width = position.width * scaleFactor;
                rect.x = (position.width - rect.width) / 2;
            }
            else
            {
                rect.height = position.height / scaleFactor;
                rect.y = (position.height - rect.height) / 2;
            }

            GUI.DrawTexture(rect, renderTexture);
        }
        GUILayout.EndVertical();
    }
}
