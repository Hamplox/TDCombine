using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ShopItem
{
    [SerializeField] public string Name;
    [SerializeField] public TowerScriptableObject TowerScriptableObject;
    [SerializeField] public Button Button;
}


public class Shop : MonoBehaviour
{
    [SerializeField] public List<ShopItem> Items = new List<ShopItem>();
    public GameObject ShopWindow;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Shop))]
public class ShopEditor : Editor
{
    SerializedObject myClassSO;
    SerializedProperty myItems;
    Shop myClass;

    public void OnEnable()
    {
        myClassSO = new SerializedObject(target);
        myClass = (Shop)target;
        for (int i = 0; i < myClass.Items.Count; i++)
        {
            if (!myClass.Items[i].Button)
            {
                myClass.Items.RemoveAt(i);
                i--;
            }
        }
        myItems = myClassSO.FindProperty("Items");
    }
    public override void OnInspectorGUI()
    {
        myClassSO.Update();
        EditorGUILayout.PropertyField(myClassSO.FindProperty("ShopWindow"));
        EditorGUILayout.BeginFoldoutHeaderGroup(true, "Shop Items");
        for (int i = 0; i < myItems.arraySize; i++)
        {

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(myItems.GetArrayElementAtIndex(i).FindPropertyRelative("TowerScriptableObject"), GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                UpdateValues(i);
            }
            EditorGUILayout.PropertyField(myItems.GetArrayElementAtIndex(i).FindPropertyRelative("Button"), GUIContent.none);
            if (GUILayout.Button("Delete"))
            {
                DeleteObject(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (GUILayout.Button("Pressed"))
        {
            CreateShopItem();
        }
        myClassSO.ApplyModifiedProperties();
    }

    public void CreateShopItem()
    {
        ShopItem shopItem = new ShopItem();
        shopItem.Name = "ShopItem";
        GameObject gameObject = new GameObject(shopItem.Name);
        gameObject.transform.parent = myClass.ShopWindow.transform;
        shopItem.Button = gameObject.AddComponent<Button>();
        gameObject.AddComponent<Image>();
        gameObject.AddComponent<RectTransform>();
        shopItem.TowerScriptableObject = null;
        myClass.Items.Add(shopItem);
    }
    public void DeleteObject(int aIndex)
    {
        DestroyImmediate(myClass.Items[aIndex].Button.gameObject);
        myClass.Items.RemoveAt(aIndex);
        myItems = myClassSO.FindProperty("Items");
    }

    public void UpdateValues(int aIndex)
    {
        var img = myClass.Items[aIndex].Button.gameObject.GetComponent<Image>();
        var test = (TowerScriptableObject)myItems.GetArrayElementAtIndex(aIndex).FindPropertyRelative("TowerScriptableObject").boxedValue;
        img.sprite = test.TowerPortait;
    }
}
#endif
