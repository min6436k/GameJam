using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string explanation;
    public Symptom tag;
    public List<Vector2Int> childSlots = new List<Vector2Int>();

}
