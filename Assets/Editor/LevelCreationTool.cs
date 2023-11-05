using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

enum PaintType
{
    eDefault,
    eWall,
    eWayPoint,
    eStartWayPoint,
    eEndWayPoint
}

public class LevelCreationTool : EditorWindow
{
    Camera camera;
    RenderTexture renderTexture;

    Ray mouseRay;
    GameObject hitObject;

    GridTool gridTool;

    bool leftButtonPressed = false;
    bool rightButtonPressed = false;

    PaintType paintType = PaintType.eDefault;

    List<string> paintTypeStrings = new List<string>();


    [MenuItem("Tools/Level Creation")]
    public static void ShowExample()
    {
        LevelCreationTool wnd = GetWindow<LevelCreationTool>();
        wnd.titleContent = new GUIContent("Level Tool");
    }

    private void Awake()
    {
        renderTexture = new RenderTexture(1920, 1080, (int)RenderTextureFormat.ARGB32); // Set an initial aspect ratio (e.g., 16:9)
        gridTool = CreateInstance<GridTool>();

        paintTypeStrings.Add("Wall");
        paintTypeStrings.Add("WayPoint");
        paintTypeStrings.Add("StartWayPoint");
        paintTypeStrings.Add("EndWayPoint");
    }

    public void OnEnable()
    {
        var cameraPrefab = GameObject.Find("Grid Tool Camera");
        if (cameraPrefab == null)
        {
            GameObject parent = new GameObject("Level_Creation");

            var newCam = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Tool Assets/Paint Tool/Grid Tool Camera.prefab");
            PrefabUtility.InstantiatePrefab(newCam);
            camera = GameObject.Find("Grid Tool Camera").GetComponent<Camera>();
            camera.transform.parent = parent.transform;
        }
        else
        {
            camera = cameraPrefab.GetComponent<Camera>();
        }

        if (!GameObject.Find("Grid"))
        {
            gridTool.MakeGrid();
        }

    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //Left and Right Main Panel
        var splitView1 = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        root.Add(splitView1);

        var leftPanel = new ListView();
        leftPanel.makeItem = () => new Label();
        leftPanel.bindItem = (item, index) => { (item as Label).text = paintTypeStrings[index]; };
        leftPanel.itemsSource = paintTypeStrings;
        splitView1.Add(leftPanel);

        leftPanel.selectionChanged += OnEnumSelectionChange;

        var rightPanel = new VisualElement();
        splitView1.Add(rightPanel);

        //Right Lower and Upper Panels
        var splitView2 = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
        rightPanel.Add(splitView2);

        //Camera Render Upper Left Panel
        var cameraImage = new Image();
        cameraImage.image = renderTexture;
        cameraImage.scaleMode = ScaleMode.StretchToFill;
        splitView2.Add(cameraImage);

        cameraImage.RegisterCallback<MouseMoveEvent, Image>(MouseClicker, cameraImage, TrickleDown.TrickleDown);
        cameraImage.RegisterCallback<MouseDownEvent, Image>(MouseDown, cameraImage, TrickleDown.TrickleDown);
        cameraImage.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);

        var lowerPanel = new VisualElement();
        var clearGrid = new Button();
        clearGrid.clicked += OnClearGrid;
        clearGrid.text = "Clear Canvas";
        lowerPanel.Add(clearGrid);
        splitView2.Add(lowerPanel);
    }

    private void Update()
    {
        if (camera == null)
            return;

        if (camera.targetTexture != renderTexture)
        {
            camera.targetTexture = renderTexture;
        }
    }

    private void OnDestroy()
    {
        DestroyImmediate(GameObject.Find("Level_Creation"));
    }

    private void OnClearGrid()
    {
        gridTool.ClearGrid();
    }

    private void OnEnumSelectionChange(IEnumerable<object> selectedItems)
    {
        var selectedEnum = selectedItems.First() as string;
        if (selectedEnum == null)
            return;

        PaintType type = GetPaintTypeFromString(selectedEnum);
        paintType = type;
    }

    PaintType GetPaintTypeFromString(string aEnumString)
    {
        if (aEnumString == "Wall")
        {
            return PaintType.eWall;
        }
        if (aEnumString == "WayPoint")
        {
            return PaintType.eWayPoint;
        }
        if (aEnumString == "StartWayPoint")
        {
            return PaintType.eStartWayPoint;
        }
        if (aEnumString == "EndWayPoint")
        {
            return PaintType.eEndWayPoint;
        }

        return PaintType.eDefault;
    }

    private void CastRay(Vector2 mousePos, Image cameraImage)
    {
        Rect cameraImageRect = cameraImage.worldBound;
        Vector2 mousePosition = mousePos;

        // Calculate the position of the mouse click within the cameraImage
        float xInCameraImage = (mousePosition.x - cameraImageRect.x);
        float yInCameraImage = (mousePosition.y - cameraImageRect.y);


        float scaleX = renderTexture.width / cameraImageRect.width;
        float scaleY = renderTexture.height / cameraImageRect.height;

        // Calculate the normalized coordinates within the RenderTexture
        float normalizedX = xInCameraImage * scaleX / renderTexture.width;
        float normalizedY = 1 - (yInCameraImage * scaleY / renderTexture.height);

        // Create a ray using the normalized coordinates
        mouseRay = camera.ViewportPointToRay(new Vector3(normalizedX, normalizedY, 0));

        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity))
        {
            hitObject = hit.collider.gameObject;
            var meshRenderer = hitObject.GetComponent<MeshRenderer>();
            if (meshRenderer && hitObject.tag == "GridCube" || hitObject.tag == "Wall" || hitObject.tag == "WayPoint" || hitObject.tag == "StartWayPoint" || hitObject.tag == "EndWayPoint")
            {
                if (rightButtonPressed)
                {
                    var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Tool Assets/Grid Prefab/m_GridCube.mat");
                    meshRenderer.material = mat;
                    if(hitObject.tag == "WayPoint")
                    {
                        GameObject.Find("Grid").GetComponent<Path>().Remove(hitObject.transform);
                    }
                    hitObject.tag = "GridCube";
                }
                else
                {
                    switch (paintType)
                    {
                        case PaintType.eWall:
                            {
                                if (hitObject.tag == "Wall")
                                    break;
                                var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Tool Assets/Grid Prefab/m_Grid_Wall.mat");
                                meshRenderer.material = mat;
                                if (hitObject.tag == "WayPoint")
                                {
                                    GameObject.Find("Grid").GetComponent<Path>().Remove(hitObject.transform);
                                }
                                hitObject.tag = "Wall";
                                break;
                            }
                        case PaintType.eWayPoint:
                            {
                                if (hitObject.tag == "WayPoint")
                                    break;

                                var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Tool Assets/Grid Prefab/m_WayPoint.mat");
                                meshRenderer.material = mat;
                                hitObject.tag = "WayPoint";
                                GameObject.Find("Grid").GetComponent<Path>().AddPath(hitObject.transform);
                                break;
                            }
                        case PaintType.eStartWayPoint:
                            {
                                if (hitObject.tag == "StartWayPoint")
                                    break;

                                var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Tool Assets/Grid Prefab/m_StartWayPoint.mat");
                                meshRenderer.material = mat;
                                if (hitObject.tag == "WayPoint")
                                {
                                    GameObject.Find("Grid").GetComponent<Path>().Remove(hitObject.transform);
                                }
                                hitObject.tag = "StartWayPoint";
                                break;
                            }
                        case PaintType.eEndWayPoint:
                            {
                                if (hitObject.tag == "EndWayPoint")
                                    break;

                                var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Tool Assets/Grid Prefab/m_EndWayPoint.mat");
                                meshRenderer.material = mat;
                                if (hitObject.tag == "WayPoint")
                                {
                                    GameObject.Find("Grid").GetComponent<Path>().Remove(hitObject.transform);
                                }
                                hitObject.tag = "EndWayPoint";
                                break;
                            }
                        default:
                            break;
                    }
                }

                Repaint();
            }
        }
    }

    void MouseClicker(MouseMoveEvent e, Image cameraImage)
    {
        if (leftButtonPressed || rightButtonPressed)
        {
            CastRay(e.mousePosition, cameraImage);
        }
    }

    private void MouseDown(MouseDownEvent e, Image cameraImage)
    {
        if (e.button == 0)
        {
            leftButtonPressed = true;
            CastRay(e.mousePosition, cameraImage);
        }
        else if (e.button == 1)
        {
            rightButtonPressed = true;
        }
    }

    private void MouseUp(MouseUpEvent e)
    {
        leftButtonPressed = false;
        rightButtonPressed = false;
    }
}
