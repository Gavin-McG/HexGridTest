using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AdventurerInfo
{
    public Sprite headSprite;
    public Sprite bodySprite;
    public ClassType classType;
}


[CreateAssetMenu(fileName = "New Adventerer Asset object", menuName = "Quest System/Adventerer Asset")]
public class AdventurerCollection : ScriptableObject
{
    public AdventurerInfo[] data;
}
