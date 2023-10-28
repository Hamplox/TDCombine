using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReplaceWithPrefab : ScriptableWizard
{
    [SerializeField]
    private GameObject mainPrefab;
    [SerializeField]
    private GameObject[] toBeReplaced;

    [MenuItem("Tools/Replace With Prefab")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Replace With Prefab", typeof(ReplaceWithPrefab), "Replace");
    }

    void OnWizardCreate()
    {
        for (int i = 0; i < toBeReplaced.Length; i++)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(mainPrefab);
            go.transform.parent = toBeReplaced[i].transform.parent;
            go.transform.localPosition = toBeReplaced[i].transform.localPosition;
            go.transform.localRotation = toBeReplaced[i].transform.localRotation;
            go.transform.localScale = toBeReplaced[i].transform.localScale;

            DestroyImmediate(toBeReplaced[i]);
        }
    }
}
