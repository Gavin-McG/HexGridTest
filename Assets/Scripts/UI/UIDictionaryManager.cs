using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIKeyValuePair
{
    public string key;
    public GameObject value;
}

public class UIDictionaryManager : MonoBehaviour
{
    [SerializeField] private List<UIKeyValuePair> uiKeyValuePairs = new List<UIKeyValuePair>();

    public static Dictionary<string, GameObject> uiDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        uiDictionary.Clear();
        
        foreach (var pair in uiKeyValuePairs)
        {
            if (!uiDictionary.ContainsKey(pair.key))
            {
                uiDictionary.Add(pair.key, pair.value);
            }
        }
    }
}