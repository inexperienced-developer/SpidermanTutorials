using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public GameObject Prefab;
}
