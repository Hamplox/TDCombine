using UnityEditor;
using UnityEngine;

public enum TowerType
{
    Basic,
    Cannon,
    Multi
}

[CreateAssetMenu(fileName = "TowerData", menuName = "Tower/TowerScriptableObject")]
public class TowerScriptableObject : ScriptableObject
{
    public GameObject TowerPrefab;
    public TowerType TowerType;
    public Sprite TowerPortait;
    
    public float Damage = 1f;
    public float ProjectailSpeed = 1f;

    //Discussion: Do we need rotation or do we just look on target?
    //public float RotationSpeed = 1f;

}

[CustomEditor(typeof(TowerScriptableObject))]
public class TowerSOEditor : Editor
{
    private SerializedProperty TowerType;
    private SerializedProperty Prefab;
    private SerializedProperty Damage;
    private SerializedProperty ProjectailSpeed;
    private SerializedProperty Portrait;

    private TowerScriptableObject myTarget;
    private void OnEnable()
    {
        TowerType = serializedObject.FindProperty("TowerType");
        Prefab = serializedObject.FindProperty("TowerPrefab");
        Damage = serializedObject.FindProperty("Damage");
        ProjectailSpeed = serializedObject.FindProperty("ProjectailSpeed");
        Portrait = serializedObject.FindProperty("TowerPortait");

        myTarget = target as TowerScriptableObject;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(Prefab);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(TowerType);   
        EditorGUILayout.PropertyField(Portrait);   
        EditorGUILayout.PropertyField(Damage);   
        EditorGUILayout.PropertyField(ProjectailSpeed);   
               
        serializedObject.ApplyModifiedProperties();
    }
}