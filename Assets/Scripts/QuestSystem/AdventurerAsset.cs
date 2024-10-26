using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Adventerer Asset object", menuName = "Quest System/Adventerer Asset")]
public class AdventurerAsset : ScriptableObject
{
    [SerializeField] public Sprite headSprite;
    [SerializeField] public Sprite bodySprite;
    [SerializeField] public ClassType classType;
}
