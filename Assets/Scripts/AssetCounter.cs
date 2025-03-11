using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetCounter : MonoBehaviour
{
    [Tooltip("Add the original asset prefab, or the first instance of the prefab present in the scene (there are no nubers after the name)")]
    public GameObject[] OriginalAssets;
    //public GameObject[] TestAsset;

    public void CountAssets()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        //TestAsset = FindObjectsOfType<GameObject>();
        Dictionary<string, int> assetDictionary = new();

        foreach (var asset in OriginalAssets)
        {
            assetDictionary.Add(asset.name.ToString(), 0);
            foreach (var obj in allObjects)
            {
                if (obj.name.ToString().Contains(asset.name.ToString()))
                    assetDictionary[asset.name.ToString()]++;
            }
        }

        foreach (var asset in assetDictionary)
        {
            Debug.Log($"{asset.Key}: count = {asset.Value}");
        }
    }

    public void SortAssets()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (var asset in OriginalAssets)
        {
            string assetName = "--PARENT--" + asset.name.ToString().Replace("_", " ");
            GameObject newParent = GameObject.Find(assetName) ? GameObject.Find(assetName) : Instantiate(new GameObject(), transform);
            newParent.name = assetName;

            foreach (var obj in allObjects)
            {
                if (obj.name.ToString().Contains(asset.name.ToString()))
                    obj.transform.parent = newParent.transform;
            }
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(AssetCounter))]
public class EditorAssetCounter : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        AssetCounter script = (AssetCounter)target;

        if (GUILayout.Button("Count Assets")) script.CountAssets();
        if (GUILayout.Button("Sort Assets")) script.SortAssets();
    }
}
#endif
